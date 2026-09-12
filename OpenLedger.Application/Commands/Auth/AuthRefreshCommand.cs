using FluentValidation;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Domain.Constants;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Commands.Auth
{
    public record AuthRefreshCommand(string RefreshToken, string AccessToken);

    public class AuthRefreshCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, ITokenGeneratorService tokenGenerator, ICurrentUserService currentUser)
    {
        public async Task<AuthResponseDto> HandleAsync(AuthRefreshCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = tokenGenerator.GetClaimsFromJwt(request.AccessToken).FindFirst(ApplicationClaims.UserId)?.Value;
            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId)) throw new UnauthorizedException("Invalid token claims.");

            var user = await userRepository.GetByIdAsync(userId, cancellationToken) ?? throw new BadRequestException("Invalid token or user.");
            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
            if (refreshToken is null || refreshToken.UserId != userId) throw new BadRequestException("Invalid token or user.");
            else if (!refreshToken.IsActive) throw new BadRequestException("Token is not active.");

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var generatedRefreshToken = tokenGenerator.GenerateRefreshToken();
            var newRefreshToken = new RefreshToken(user.Id, generatedRefreshToken.RefreshToken, generatedRefreshToken.RefreshTokenExpires, currentUser.IpAddress, currentUser.UserAgent);

            refreshToken.Revoke(currentUser.IpAddress, "Replaced by new token", request.RefreshToken);
            refreshTokenRepository.Update(refreshToken);
            await refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(jwt, newRefreshToken.Token, newRefreshToken.ExpiresAt);
        }
    }

    public class AuthRefreshCommandValidator : AbstractValidator<AuthRefreshCommand>
    {
        public AuthRefreshCommandValidator()
        {
            RuleFor(r => r.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MaximumLength(100).WithMessage("Refresh token is too long.");

            RuleFor(r => r.AccessToken)
                .NotEmpty().WithMessage("Access token is required.");
        }
    }
}
