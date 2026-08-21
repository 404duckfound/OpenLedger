using Microsoft.EntityFrameworkCore;
using OpenLedger.Application.Interfaces.Repository.Customs;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Infrastructure.Contexts;

namespace OpenLedger.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
    {
        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            return;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token, cancellationToken);
        }

        public async Task<List<RefreshToken>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await context.RefreshTokens.Where(r=>r.UserId == userId).ToListAsync(cancellationToken);
        }

        public Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            context.RefreshTokens.Update(refreshToken);
            return Task.CompletedTask;
        }
    }
}
