namespace OpenLedger.Application.Interfaces.Repositories.Base
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}