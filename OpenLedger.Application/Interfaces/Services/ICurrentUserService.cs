namespace OpenLedger.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        string IpAddress { get; }
        string UserAgent { get; }
    }
}
