using MediatR;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Domain.Enums;

namespace OpenLedger.Application.Commands.TenantCommands.Create
{
    public class TenantCreateCommandHandler(ICurrentUserService currentUserService, IUserRepository userRepository, IUnitOfWork unitOfWork, ITenantRepository tenantRepository, ITokenGenerator tokenGenerator) : IRequestHandler<TenantCreateCommand, AccessTokenDto>
    {
        public async Task<AccessTokenDto> Handle(TenantCreateCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(currentUserService.UserId, cancellationToken) ?? throw new BadRequestException("User not found.");
            if (user.TenantId != Guid.Empty) throw new BadRequestException("User already has a tenant.");

            var tenant = new Tenant(request.Name, request.Email, request.TaxNumber, request.TaxOffice, request.PhoneNumber, request.Address);
            user.SetTenantId(tenant.Id);
            user.SetRole(UserRole.Admin);

            var accessToken = tokenGenerator.GenerateJwtToken(user);

            userRepository.Update(user);
            await tenantRepository.AddAsync(tenant, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AccessTokenDto(accessToken);
        }
    }
}
