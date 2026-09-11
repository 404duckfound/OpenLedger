using FluentValidation;
using MediatR;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Domain.Entities.Auth;
using System.Text.RegularExpressions;

namespace OpenLedger.Application.Commands.AuthCommands
{
    public record AuthLoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;

    public class AuthLoginCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, ICurrentUserService currentUser) : IRequestHandler<AuthLoginCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(AuthLoginCommand request, CancellationToken cancellationToken)
        {
            User user = await userRepository.GetByEmailAsync(request.Email, cancellationToken) ?? throw new UnauthorizedException("Invalid email or password");
            if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash)) throw new UnauthorizedException("Invalid email or password");

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var generatedRefreshToken = tokenGenerator.GenerateRefreshToken();
            var refreshToken = new RefreshToken(user.Id, generatedRefreshToken.RefreshToken, generatedRefreshToken.RefreshTokenExpires, currentUser.IpAddress, currentUser.UserAgent);

            await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(jwt, refreshToken.Token, refreshToken.ExpiresAt);
        }
    }

    public class AuthLoginCommandValidator : AbstractValidator<AuthLoginCommand>
    {
        private static readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", RegexOptions.Compiled);
        public AuthLoginCommandValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(PasswordRegex).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.")
                .Length(8, 72).WithMessage("Password must be between 8 and 72 characters long.");
        }
    }
}
