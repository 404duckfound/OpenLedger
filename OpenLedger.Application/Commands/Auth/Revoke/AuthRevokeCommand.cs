using MediatR;

namespace OpenLedger.Application.Commands.Auth.Revoke
{
    public record AuthRevokeCommand(string RefreshToken) : IRequest;
}