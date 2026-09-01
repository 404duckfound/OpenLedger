using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using OpenLedger.Application.Dtos;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Application.Options;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Commands.Auth.Refresh
{
    public class RefreshCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, ITokenGenerator tokenGenerator, ICurrentUserService currentUser, IOptions<TokenOptions> options) : IRequestHandler<RefreshCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = tokenGenerator.GetClaimsFromJwt(request.AccessToken).FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new UnauthorizedAccessException("Invalid access token claims.");
            var userId = Guid.Parse(userIdClaim);

            var user = await userRepository.GetByIdAsync(userId, cancellationToken) ?? throw new UnauthorizedAccessException("User not found.");
            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid refresh token.");
            if (refreshToken.UserId != userId) throw new UnauthorizedAccessException("Wrong user.");

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var newRefreshToken = new RefreshToken(user.Id, tokenGenerator.GenerateRefreshToken(), DateTime.UtcNow.AddDays(options.Value.RefreshExpiresDays), currentUser.IpAddress, currentUser.UserAgent);

            refreshToken.Revoke(currentUser.IpAddress, "Replaced by new token.", request.RefreshToken);

            await refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
            await refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(jwt, newRefreshToken.Token, newRefreshToken.ExpiresAt);
        }
    }
}
