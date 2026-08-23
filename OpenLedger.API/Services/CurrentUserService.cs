using OpenLedger.Application.Interfaces.Services;

namespace OpenLedger.API.Services
{
    public class CurrentUserService(IHttpContextAccessor context) : ICurrentUserService
    {
        public string IpAddress => context.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        public string UserAgent => context.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "Unknown";
    }
}
