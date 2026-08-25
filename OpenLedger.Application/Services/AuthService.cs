using Microsoft.Extensions.Options;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Application.Options;
using OpenLedger.Domain.Entities.Auth;
using System.Security.Claims;

namespace OpenLedger.Application.Services
{
    public class AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, IPasswordHasher passwordHasher, ICurrentUserService currentUser, IOptions<TokenOptions> tokenOptions) : IAuthService
    {
        private readonly int RefreshTimer = tokenOptions.Value.RefreshExpiresDays;
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
        {
            User user = await userRepository.GetByEmailAsync(request.Email, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid email or password");

            if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash)) throw new UnauthorizedAccessException("Invalid email or password");

            var res = await CreateResponseAsync(user, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return res;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
        {
            if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken)) throw new InvalidOperationException("Email is already exists.");

            var passwordHash = passwordHasher.HashPassword(request.Password);

            var user = new User(request.Name, request.Email, passwordHash);

            var res = await CreateResponseAsync(user, cancellationToken);

            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return res;
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
        {
            var claims = tokenGenerator.GetClaimsFromJwt(request.AccessToken);
            var userIdClaim = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("Invalid access token claims.");
            var userId = Guid.Parse(userIdClaim);

            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid refresh token.");
            var user = await userRepository.GetByIdAsync(userId, cancellationToken) ?? throw new UnauthorizedAccessException("User not found.");

            if (userId != user.Id) throw new UnauthorizedAccessException("User id don't match.");

            var res = await CreateResponseAsync(user, cancellationToken);

            refreshToken.Revoke(currentUser.IpAddress, "Replaced by new token.", res.RefreshToken);

            await refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return res;
        }

        public async Task RevokeTokenAsync(RevokeTokenRequestDto request, CancellationToken cancellationToken = default)
        {
            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid refresh token.");

            refreshToken.Revoke(currentUser.IpAddress, "Revoked by user.");
            await refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return;
        }

        private async Task<AuthResponseDto> CreateResponseAsync(User user, CancellationToken cancellationToken = default)
        {
            string jwt = tokenGenerator.GenerateJwtToken(user);
            RefreshToken refreshToken = new(user.Id, tokenGenerator.GenerateRefreshToken(), DateTime.UtcNow.AddDays(RefreshTimer), currentUser.IpAddress, currentUser.UserAgent);

            await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            return new AuthResponseDto(jwt, refreshToken.Token!, refreshToken.ExpiresAt);
        }
    }
}
