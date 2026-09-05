using MediatR;

namespace OpenLedger.Application.Commands.Tenant.Create
{
    public record TenantCreateCommand(string Name, string Description) : IRequest<Guid>;
}
