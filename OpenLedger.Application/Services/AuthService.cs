using Microsoft.Extensions.Options;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Infrastructure.Options;
using System.Security.Claims;

namespace OpenLedger.Application.Services
{
    public class AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, IPasswordHasher passwordHasher, IOptions<TokenOptions> tokenOptions) : IAuthService
    {
        public readonly int RefreshTimer = tokenOptions.Value.RefreshExpiresDays;
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, ICurrentUserService currentUser, CancellationToken cancellationToken = default)
        {
            User user = await userRepository.GetByEmailAsync(request.Email, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid email or password");

            if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash)) throw new UnauthorizedAccessException("Invalid email or password");

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var refresh = tokenGenerator.GenerateRefreshToken();

            var refreshObj = new RefreshToken(user.Id, refresh, DateTime.UtcNow.AddDays(RefreshTimer), currentUser.IpAddress, currentUser.UserAgent);

            await refreshTokenRepository.AddAsync(refreshObj, cancellationToken);

            var res = new AuthResponseDto(jwt, refresh, refreshObj.ExpiresAt.GetValueOrDefault());

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return res;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, ICurrentUserService currentUser, CancellationToken cancellationToken = default)
        {
            if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken)) throw new ArgumentException("Email is already exists.");

            var passwordHash = passwordHasher.HashPassword(request.Password);

            var user = new User(request.Name, request.Email, passwordHash);

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var refresh = tokenGenerator.GenerateRefreshToken();

            var refreshObj = new RefreshToken(user.Id, refresh, DateTime.UtcNow.AddDays(RefreshTimer), currentUser.IpAddress, currentUser.UserAgent);

            await userRepository.AddAsync(user, cancellationToken);
            await refreshTokenRepository.AddAsync(refreshObj, cancellationToken);

            var res = new AuthResponseDto(jwt, refresh, refreshObj.ExpiresAt.GetValueOrDefault());

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return res;
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, ICurrentUserService currentUser, CancellationToken cancellationToken = default)
        {
            var claims = tokenGenerator.GetClaimsFromJwt(request.AccessToken);
            var userIdClaim = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("Invalid access token claims.");
            var userId = Guid.Parse(userIdClaim);

            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid refresh token.");
            var user = await userRepository.GetByIdAsync(userId, cancellationToken) ?? throw new UnauthorizedAccessException("User not found.");

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var newRefresh = tokenGenerator.GenerateRefreshToken();

            var refreshObj = new RefreshToken(userId, newRefresh, DateTime.UtcNow.AddDays(RefreshTimer), currentUser.IpAddress, currentUser.UserAgent);

            await refreshTokenRepository.AddAsync(refreshObj, cancellationToken);

            refreshToken.Revoke(currentUser.IpAddress, refreshToken.Token);
            await refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

            var res = new AuthResponseDto(jwt, newRefresh, refreshObj.ExpiresAt.GetValueOrDefault());

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return res;
        }

        public async Task RevokeTokenAsync(RevokeTokenRequestDto request, ICurrentUserService currentUser,, CancellationToken cancellationToken = default)
        {
            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid refresh token.");

            refreshToken.Revoke(currentUser.IpAddress, refreshToken.Token);
            await refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return;
        }
    }
}
