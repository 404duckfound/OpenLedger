using OpenLedger.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLedger.Domain.Entities.Auth
{
    public class Tenant(string name) : BaseEntity
    {
        public string Name { get; private set; } = name;        
        public DateTime SubscriptionExpiration {  get; private set; } = DateTime.UtcNow;

        public string? Email {  get; private set; }
        public string? TaxNumber { get; private set; }
        public string? TaxOffice {  get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? Address { get; private set; }

        // Domain Functions
        public bool IsSubscriptionEnd { get => SubscriptionExpiration <= DateTime.UtcNow; }
    }
}
