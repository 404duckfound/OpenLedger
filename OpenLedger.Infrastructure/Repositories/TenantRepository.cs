using Microsoft.EntityFrameworkCore;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Infrastructure.Contexts;

namespace OpenLedger.Infrastructure.Repositories
{
    public class TenantRepository(AppDbContext context) : ITenantRepository
    {
        public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
        {
            await context.Tenants.AddAsync(tenant, cancellationToken);
        }

        public async Task<Tenant?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await context.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == guid, cancellationToken);
        }

        public void Update(Tenant tenant)
        {
            context.Tenants.Update(tenant);
        }
    }
}
