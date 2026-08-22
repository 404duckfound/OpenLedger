using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Interfaces.Repository.Customs
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
        Task<List<RefreshToken>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    }
}