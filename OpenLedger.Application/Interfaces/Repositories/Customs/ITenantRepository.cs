using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Interfaces.Repositories.Customs
{
    public interface ITenantRepository
    {
        Task<bool> IsTenantExistsAsync(Guid tenantId, CancellationToken cancellationToken = default);
        Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default);
    }
}
