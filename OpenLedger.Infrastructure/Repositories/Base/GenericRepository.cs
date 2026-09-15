using Microsoft.EntityFrameworkCore;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Infrastructure.Contexts;

namespace OpenLedger.Infrastructure.Repositories.Base
{
    public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
    {
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await context.Set<T>().AddAsync(entity, cancellationToken);
        }
        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }
        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().ToListAsync(cancellationToken);
        }
        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().FindAsync(id, cancellationToken);
        }
        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
    }
}