using MediatR;

namespace OpenLedger.Application.Commands.Auth.Revoke
{
    public record RevokeCommand(string RefreshToken) : IRequest;
}