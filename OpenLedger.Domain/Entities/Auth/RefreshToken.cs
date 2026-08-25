using OpenLedger.Domain.Base;

namespace OpenLedger.Domain.Entities.Auth
{
    public class RefreshToken(Guid userId, string token, DateTime expiresAt, string createdByIp, string userAgent) : BaseEntity
    {
        public Guid UserId { get; private set; } = userId;
        public string Token { get; private set; } = token;
        public string UserAgent { get; private set; } = userAgent;
        public DateTime ExpiresAt { get; private set; } = expiresAt;
        public string CreatedByIp { get; private set; } = createdByIp;

        public string? RevokedByIp { get; private set; }
        public DateTime RevokedAt { get; private set; }
        public string? ReplacedByToken { get; private set; }
        public string? RevokeReason { get; private set; }

        // Domain Functions
        public bool IsRevoked { get => RevokedAt <= DateTime.MinValue; }
        public bool IsExpired { get => ExpiresAt < DateTime.UtcNow; }
        public bool IsActive { get => !IsRevoked && !IsExpired; }

        public void Revoke(string revokedByIp, string reason, string? replacedByToken = null)
        {
            if (IsRevoked) throw new Exception("Token is already revoked.");

            RevokedAt = DateTime.UtcNow;
            RevokedByIp = revokedByIp;
            ReplacedByToken = replacedByToken;
            RevokeReason = reason;
        }
    }
}
