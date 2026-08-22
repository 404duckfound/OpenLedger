using OpenLedger.Application.Dtos.Auth;

namespace OpenLedger.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto request, string createdByIp, string userAgent, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request, string createdByIp, string userAgent, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
        Task RevokeTokenAsync(RevokeTokenRequestDto request, CancellationToken cancellationToken = default);
    }
}