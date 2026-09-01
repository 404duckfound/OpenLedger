using MediatR;
using OpenLedger.Application.Dtos;

namespace OpenLedger.Application.Commands.Auth.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
}
