using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Infrastructure.Contexts;

namespace OpenLedger.Infrastructure.Repositories.Base
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
            return; 
        }
    }
}