namespace OpenLedger.Domain.Base
{
    public class BaseEntity
    {
        public Guid Id { get; private set; } = Guid.CreateVersion7();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}
