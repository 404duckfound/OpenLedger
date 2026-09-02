namespace OpenLedger.Domain.Base
{
    public class BaseTenantEntity : BaseEntity
    {
        public Guid TenantId { get; protected set; }
    }
}
