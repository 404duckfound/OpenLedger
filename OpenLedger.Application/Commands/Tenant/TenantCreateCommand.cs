using FluentValidation;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Domain.Enums;
using System.Text.RegularExpressions;

namespace OpenLedger.Application.Commands.Tenant
{
    public record TenantCreateCommand(string Name, string? Email, string? TaxNumber, string? TaxOffice, string? PhoneNumber, string? Address);

    public class TenantCreateCommandHandler(ICurrentUserService currentUserService, IUserRepository userRepository, IUnitOfWork unitOfWork, ITenantRepository tenantRepository, ITokenGeneratorService tokenGenerator)
    {
        public async Task<AccessTokenDto> HandleAsync(TenantCreateCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(currentUserService.UserId, cancellationToken) ?? throw new BadRequestException("User not found.");
            if (user.TenantId != Guid.Empty) throw new BadRequestException("User already has a tenant.");

            var tenant = new Domain.Entities.Auth.Tenant(request.Name, request.Email, request.TaxNumber, request.TaxOffice, request.PhoneNumber, request.Address);
            user.SetTenantId(tenant.Id);
            user.SetRole(UserRole.Admin);

            var accessToken = tokenGenerator.GenerateJwtToken(user);

            userRepository.Update(user);
            await tenantRepository.AddAsync(tenant, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AccessTokenDto(accessToken);
        }
    }

    public class TenantCreateCommandValidator : AbstractValidator<TenantCreateCommand>
    {
        private readonly Regex PhoneNumberRegex = new(@"^\+?[1-9]\d*$", RegexOptions.Compiled);
        public TenantCreateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tenant name is required.")
                .MaximumLength(50).WithMessage("Tenant name must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.TaxNumber)
                .MaximumLength(50).WithMessage("Tax number must not exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.TaxNumber));

            RuleFor(x => x.TaxOffice)
                .MaximumLength(50).WithMessage("Tax office must not exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.TaxOffice));

            RuleFor(x => x.PhoneNumber)
                .Matches(PhoneNumberRegex).WithMessage("Invalid phone number format.")
                .MaximumLength(50).WithMessage("Phone number must not exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.Address)
                .MaximumLength(100).WithMessage("Address must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Address));
        }
    }
}
