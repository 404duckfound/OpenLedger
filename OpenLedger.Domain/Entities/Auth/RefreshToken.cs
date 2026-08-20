using OpenLedger.Domain.Base;

namespace OpenLedger.Domain.Entities.Auth
{
    public class RefreshToken : BaseEntity
    {
        public Guid? UserId { get; private set; }

        public string? Token { get; private set; }

        public DateTime? RevokedAt { get; private set; }
        public DateTime? ExpiresAt { get; private set; }

        public string? CreatedByIp { get; private set; }        
        public string? RevokedByIp { get; private set; }

        public string? UserAgent { get; private set; }
        public string? ReplacedByToken { get; private set; }

        public bool IsRevoked { get => RevokedAt != null; }
        public bool IsExpired { get => ExpiresAt < DateTime.UtcNow; }
        public bool IsActive { get => !IsRevoked && !IsExpired; }

        public RefreshToken(Guid userId, string token, DateTime expiresAt, string createdByIp, string userAgent)
        {   if (userId == Guid.Empty) throw new ArgumentException("User id cannot be null or empty");
            else if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException("Token cannot be null or empty");
            else if (expiresAt <= DateTime.MinValue) throw new ArgumentException("Expires at might be possible value");
            else if (string.IsNullOrWhiteSpace(createdByIp)) throw new ArgumentException("Created by ip cannot be null");
            else if (string.IsNullOrWhiteSpace(userAgent)) throw new ArgumentException("User agent cannot be null");

            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            CreatedByIp = createdByIp;
            UserAgent = userAgent;
        }
        public void Revoke(string revokedByIp, string? replacedByToken = null)
        {
            if (IsRevoked)
            {
                throw new InvalidOperationException("Token is already revoked");
            }
            else if (string.IsNullOrWhiteSpace(revokedByIp)) throw new ArgumentException("Revoked by ip cannot be null");
            RevokedAt = DateTime.UtcNow;
            RevokedByIp = revokedByIp;
            ReplacedByToken = replacedByToken;
        }
    }
}
