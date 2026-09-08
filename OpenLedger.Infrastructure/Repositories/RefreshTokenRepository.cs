using Microsoft.EntityFrameworkCore;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Infrastructure.Contexts;

namespace OpenLedger.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
    {
        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }
        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Token == token, cancellationToken);
        }
        public async Task<List<RefreshToken>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await context.RefreshTokens
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .ToListAsync(cancellationToken);
        }
        public async Task RevokeAllByUserIdAsync(Guid userId, string revokedByIp, CancellationToken cancellationToken = default)
        {
            var refreshTokens = await context.RefreshTokens
                .Where(r => r.UserId == userId && r.RevokedAt <= DateTime.MinValue && r.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(cancellationToken);
            
            foreach (var token in refreshTokens)
            {
                token.Revoke(revokedByIp);
            }
        }
        public void Update(RefreshToken refreshToken)
        {
            context.RefreshTokens.Update(refreshToken);
        }
    }
}
