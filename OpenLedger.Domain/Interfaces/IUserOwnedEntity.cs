namespace OpenLedger.Domain.Interfaces
{
    public interface IUserOwnedEntity
    {
        Guid UserId { get; }
    }
}
