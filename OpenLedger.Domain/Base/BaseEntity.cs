namespace OpenLedger.Domain.Base
{
    public class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.CreateVersion7();
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    }
}
