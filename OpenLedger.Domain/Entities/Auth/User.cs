using OpenLedger.Domain.Base;
using OpenLedger.Domain.Enums;

namespace OpenLedger.Domain.Entities.Auth
{
    public class User(string name, string email, string passwordHash) : BaseTenantEntity
    {
        public string Name { get; private set; } = name;
        public string Email { get; private set; } = email;
        public string PasswordHash { get; private set; } = passwordHash;

        public bool IsVerified { get; private set; }
        public UserRole Role { get; private set; }
    }
}
