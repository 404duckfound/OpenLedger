using MediatR;

namespace OpenLedger.Application.Commands.AuthCommands.Logout
{
    public record AuthLogoutCommand(string RefreshToken) : IRequest<string>;
}