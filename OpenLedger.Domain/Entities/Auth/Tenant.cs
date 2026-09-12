using OpenLedger.Domain.Base;

namespace OpenLedger.Domain.Entities.Auth
{
    public class Tenant(string name, string? email=null, string? taxNumber=null, string? taxOffice=null, string? phoneNumber=null, string? address=null) : BaseEntity
    {
        public string Name { get; private set; } = name;
        public string? Email { get; private set; } = email;
        public string? TaxNumber { get; private set; } = taxNumber;
        public string? TaxOffice { get; private set; } = taxOffice;
        public string? PhoneNumber { get; private set; } = phoneNumber;
        public string? Address { get; private set; } = address;
        public DateTime SubscriptionExpiration { get; private set; } = DateTime.UtcNow.AddMonths(1);

        // Domain Functions
        public bool IsSubscriptionEnd { get => SubscriptionExpiration <= DateTime.MinValue ? true : SubscriptionExpiration <= DateTime.UtcNow; }

        public void Update(string name, string? email = null, string? taxNumber = null, string? taxOffice = null, string? phoneNumber = null, string? address = null)
        {
            Name = name;
            Email = email;
            TaxNumber = taxNumber;
            TaxOffice = taxOffice;
            PhoneNumber = phoneNumber;
            Address = address;
        }
    }
}
