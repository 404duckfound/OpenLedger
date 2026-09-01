using MediatR;
using OpenLedger.Application.Dtos;

namespace OpenLedger.Application.Commands.Auth.Register
{
    public record RegisterCommand(string Email, string Name, string Password, string ConfirmPassword) : IRequest<AuthResponseDto>;
}
