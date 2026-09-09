using MediatR;
using OpenLedger.Application.Dtos.Auth;

namespace OpenLedger.Application.Commands.AuthCommands.Refresh
{
    public record AuthRefreshCommand(string RefreshToken, string AccessToken) : IRequest<AuthResponseDto>;
}
