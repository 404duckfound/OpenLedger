using MediatR;
using OpenLedger.Application.Dtos;

namespace OpenLedger.Application.Commands.Auth.Refresh
{
    public record AuthRefreshCommand(string RefreshToken, string AccessToken) : IRequest<AuthResponseDto>;
}
