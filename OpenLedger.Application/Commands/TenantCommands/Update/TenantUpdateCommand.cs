using MediatR;

namespace OpenLedger.Application.Commands.TenantCommands.Update
{
    public record TenantUpdateCommand(string Name, string? Email, string? TaxNumber, string? TaxOffice, string? PhoneNumber, string? Address) : IRequest<string>;
}
