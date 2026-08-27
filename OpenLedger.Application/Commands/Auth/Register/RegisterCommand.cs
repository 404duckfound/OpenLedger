using MediatR;

namespace OpenLedger.Application.Commands.Auth.Register
{
    public record RegisterCommand(string Email, string Name, string Password, string ConfirmPassword) : IRequest<AuthResponseDto>;
}
