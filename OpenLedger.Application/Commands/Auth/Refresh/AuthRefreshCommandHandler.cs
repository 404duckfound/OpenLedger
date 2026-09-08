using MediatR;
using OpenLedger.Application.Dtos;
using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Domain.Constants;
using OpenLedger.Domain.Entities.Auth;
using System.Security.Claims;

namespace OpenLedger.Application.Commands.Auth.Refresh
{
    public class AuthRefreshCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, ITokenGenerator tokenGenerator, ICurrentUserService currentUser) : IRequestHandler<AuthRefreshCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(AuthRefreshCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = tokenGenerator.GetClaimsFromJwt(request.AccessToken).FindFirst(ApplicationClaims.UserId)?.Value;
            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId)) throw new UnauthorizedException("Invalid token claims.");

            var user = await userRepository.GetByIdAsync(userId, cancellationToken) ?? throw new BadRequestException("Invalid token or user.");
            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
            if (refreshToken is null || refreshToken.UserId != userId) throw new BadRequestException("Invalid token or user.");
            else if (!refreshToken.IsActive)
            {
                if (!refreshToken.IsRevoked && refreshToken.IsExpired)
                {
                    refreshToken.Revoke(currentUser.IpAddress, "Expired token");
                    refreshTokenRepository.Update(refreshToken);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }
                throw new BadRequestException("Token is not active.");
            }

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var generatedRefreshToken = tokenGenerator.GenerateRefreshToken();
            var newRefreshToken = new RefreshToken(user.Id, generatedRefreshToken.Token, generatedRefreshToken.ExpiresAt, currentUser.IpAddress, currentUser.UserAgent);

            refreshToken.Revoke(currentUser.IpAddress, "Replaced by new token", request.RefreshToken);
            refreshTokenRepository.Update(refreshToken);
            await refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(jwt, newRefreshToken.Token, newRefreshToken.ExpiresAt);
        }
    }
}
