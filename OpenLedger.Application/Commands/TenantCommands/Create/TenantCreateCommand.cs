using MediatR;
using OpenLedger.Application.Dtos.Auth;

namespace OpenLedger.Application.Commands.TenantCommands.Create
{
    public record TenantCreateCommand(string Name, string? Email, string? TaxNumber, string? TaxOffice, string? PhoneNumber, string? Address) : IRequest<AccessTokenDto>;
}
