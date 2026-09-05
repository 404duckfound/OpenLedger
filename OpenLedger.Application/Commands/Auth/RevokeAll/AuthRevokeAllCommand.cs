using MediatR;

namespace OpenLedger.Application.Commands.Auth.RevokeAll
{
    public record AuthRevokeAllCommand() : IRequest;
}
