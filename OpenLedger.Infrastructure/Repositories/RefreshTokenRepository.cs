using OpenLedger.Application.Interfaces.Repository.Customs;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Infrastructure.Repositories
{
    public class RefreshTokenRepository() : IRefreshTokenRepository
    {
        public Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<RefreshToken>> GetByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken> RevokeRefreshTokenAsync(RefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
