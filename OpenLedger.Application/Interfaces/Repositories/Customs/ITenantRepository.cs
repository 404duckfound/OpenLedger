using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Interfaces.Repositories.Customs
{
    public interface ITenantRepository
    {
        Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default);
        Task<Tenant?> GetByIdAsync(Guid guid,CancellationToken cancellationToken = default);
        void Update(Tenant tenant);
    }
}
