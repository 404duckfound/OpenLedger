using Microsoft.Extensions.Options;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Interfaces.Auth;
using OpenLedger.Application.Interfaces.Repository.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Infrastructure.Options;

namespace OpenLedger.Infrastructure.Services
{
    public class AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, ITokenGenerator tokenGenerator, IOptions<TokenOptions> tokenOptions) : IAuthService
    {
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, string createdByIp, string userAgent, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken) ?? throw new ArgumentNullException("Email or password is incorrect.");

            if (false) throw new ArgumentException("Email or password is incorrect.");

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var refresh = tokenGenerator.GenerateRefreshToken();

            var refreshObj = new RefreshToken(user.Id, refresh, DateTime.UtcNow.AddDays(tokenOptions.Value.RefreshExpiresDays), createdByIp, userAgent);

            await refreshTokenRepository.AddAsync(refreshObj,cancellationToken);

            var res = new AuthResponseDto
            {
                AccessToken = jwt,
                RefreshToken = refresh,
                RefreshTokenExpires = refreshObj.ExpiresAt.GetValueOrDefault(),
            };

            return res;
        }

        public Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAsync(RegisterRequestDto request, string createdByIp, string userAgent, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RevokeTokenAsync(RevokeTokenRequestDto request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
