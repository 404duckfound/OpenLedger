using MediatR;
using OpenLedger.Application.Dtos.Auth;

namespace OpenLedger.Application.Commands.AuthCommands.Login
{
    public record AuthLoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
}
