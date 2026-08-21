using Microsoft.EntityFrameworkCore;
using OpenLedger.Application.Interfaces.Repository.Customs;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Infrastructure.Contexts;

namespace OpenLedger.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        { 
            await context.Users.AddAsync(user, cancellationToken);
            return;
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}
