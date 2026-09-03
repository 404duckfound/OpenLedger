using Microsoft.EntityFrameworkCore;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Infrastructure.Contexts;

namespace OpenLedger.Infrastructure.Repositories
{
    internal class TenantRepository(AppDbContext context) : ITenantRepository
    {
        public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
        {
           await context.Tenants.AddAsync(tenant, cancellationToken);
        }

        public async Task<bool> IsTenantExistsAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
           return await context.Tenants.AnyAsync(t => t.Id == tenantId, cancellationToken);
        }
    }
}
