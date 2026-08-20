using OpenLedger.Domain.Base;
using OpenLedger.Domain.Enums;

namespace OpenLedger.Domain.Entities.Auth
{
    public class User(string name, string email, string passwordHash) : BaseEntity
    {
        public Guid TenantId = Guid.Empty;
        public string Name { get; set; } = name;
        public string Email { get; set; } = email;
        public string PasswordHash { get; set; } = passwordHash;
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsVerified { get; set; } = false;
    }
}
