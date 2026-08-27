using MediatR;

namespace OpenLedger.Application.Commands.Auth.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
}
