namespace OpenLedger.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        Guid TenantId { get; }
        Guid UserId { get; }
        string IpAddress { get; }
        string UserAgent { get; }
    }
}
