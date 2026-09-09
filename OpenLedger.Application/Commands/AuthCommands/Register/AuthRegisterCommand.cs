using MediatR;
using OpenLedger.Application.Dtos.Auth;

namespace OpenLedger.Application.Commands.AuthCommands.Register
{
    public record AuthRegisterCommand(string Email, string Name, string Password, string ConfirmPassword) : IRequest<AuthResponseDto>;
}
