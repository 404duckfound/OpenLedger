using Microsoft.Extensions.Options;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Infrastructure.Options;

namespace OpenLedger.Infrastructure.Services
{
    public class AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, IPasswordHasher passwordHasher, IOptions<TokenOptions> tokenOptions) : IAuthService
    {
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, string createdByIp, string userAgent, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken) ?? throw new ArgumentNullException("Email or password is incorrect.");

            if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash)) throw new ArgumentException("Email or password is incorrect.");

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var refresh = tokenGenerator.GenerateRefreshToken();

            var refreshObj = new RefreshToken(user.Id, refresh, DateTime.UtcNow.AddDays(tokenOptions.Value.RefreshExpiresDays), createdByIp, userAgent);

            await refreshTokenRepository.AddAsync(refreshObj, cancellationToken);

            var res = new AuthResponseDto
            {
                AccessToken = jwt,
                RefreshToken = refresh,
                RefreshTokenExpires = refreshObj.ExpiresAt.GetValueOrDefault(),
            };

            await unitOfWork.SaveChangesAsync();

            return res;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, string createdByIp, string userAgent, CancellationToken cancellationToken = default)
        {
            if (await userRepository.ExistsByEmailAsync(request.Email)) throw new ArgumentException("Email is already exists.");

            var passwordHash = passwordHasher.HashPassword(request.Password);

            var user = new User(request.Name, request.Email, passwordHash);

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var refresh = tokenGenerator.GenerateRefreshToken();

            var refreshObj = new RefreshToken(user.Id, refresh, DateTime.UtcNow.AddDays(tokenOptions.Value.RefreshExpiresDays), createdByIp, userAgent);

            await refreshTokenRepository.AddAsync(refreshObj, cancellationToken);

            var res = new AuthResponseDto
            {
                AccessToken = jwt,
                RefreshToken = refresh,
                RefreshTokenExpires = refreshObj.ExpiresAt.GetValueOrDefault(),
            };

            await unitOfWork.SaveChangesAsync();

            return res;
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, User user, string createdByIp, string userAgent, CancellationToken cancellationToken = default)
        {
            var refresh = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken) ?? throw new ArgumentNullException("Refresh token is wrong.");

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var newRefresh = tokenGenerator.GenerateRefreshToken();

            var refreshObj = new RefreshToken(user.Id, newRefresh, DateTime.UtcNow.AddDays(tokenOptions.Value.RefreshExpiresDays), createdByIp, userAgent);

            await refreshTokenRepository.AddAsync(refreshObj, cancellationToken);

            var res = new AuthResponseDto
            {
                AccessToken = jwt,
                RefreshToken = newRefresh,
                RefreshTokenExpires = refreshObj.ExpiresAt.GetValueOrDefault(),
            };

            await unitOfWork.SaveChangesAsync();

            return res;
        }

        public Task RevokeTokenAsync(RevokeTokenRequestDto request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
