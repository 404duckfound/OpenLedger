using MediatR;

namespace OpenLedger.Application.Commands.AuthCommands.LogoutAll
{
    public record AuthLogoutAllCommand() : IRequest<string>;
}
