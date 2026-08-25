using OpenLedger.Application.Dtos.Auth.Response;
using OpenLedger.Application.Dtos.Auth.Request;


namespace OpenLedger.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
        Task RevokeTokenAsync(RevokeTokenRequestDto request, CancellationToken cancellationToken = default);
    }
}