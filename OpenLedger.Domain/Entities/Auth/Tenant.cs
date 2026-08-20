using OpenLedger.Domain.Base;

namespace OpenLedger.Domain.Entities.Auth
{
    public class Tenant : BaseEntity
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxOffice { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime SubscriptionExpiration { get; set; } = DateTime.UtcNow;
        public bool IsSubscriptionEnd { get => SubscriptionExpiration <= DateTime.UtcNow; }
    }
}
