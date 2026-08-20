using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Interfaces.Repository.Customs
{
    public interface IRefreshTokenRepository
    {
        public Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken refreshToken);
        public Task<RefreshToken> RevokeRefreshTokenAsync(RefreshToken refreshToken);
        public Task<List<RefreshToken>> GetByUserIdAsync(Guid userId);
        public Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
    }
}