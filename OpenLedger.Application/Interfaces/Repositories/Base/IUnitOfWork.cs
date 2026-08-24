namespace OpenLedger.Application.Interfaces.Repositories.Base
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}