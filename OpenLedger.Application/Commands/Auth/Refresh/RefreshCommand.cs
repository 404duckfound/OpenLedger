using MediatR;

namespace OpenLedger.Application.Commands.Auth.Refresh
{
    public record RefreshCommand(string RefreshToken, string AccessToken) : IRequest<AuthResponseDto>;
}
