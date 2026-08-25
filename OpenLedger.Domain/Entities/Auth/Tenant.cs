using OpenLedger.Domain.Base;

namespace OpenLedger.Domain.Entities.Auth
{
    public class Tenant(string name) : BaseEntity
    {
        public string Name { get; private set; } = name;
        public DateTime SubscriptionExpiration { get; private set; } = DateTime.UtcNow.AddMonths(1);

        public string? Email { get; private set; }
        public string? TaxNumber { get; private set; }
        public string? TaxOffice { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? Address { get; private set; }

        // Domain Functions
        public bool IsSubscriptionEnd { get => SubscriptionExpiration <= DateTime.MinValue ? true : SubscriptionExpiration <= DateTime.UtcNow; }
    }
}
