using OpenLedger.Application.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OpenLedger.API.Services
{
    public class CurrentUserService(IHttpContextAccessor context) : ICurrentUserService
    {
        public Guid TenantId
        {
            get
            {
                if (Guid.TryParse(context.HttpContext?.User.FindFirstValue("TenantId"), out Guid TenantId))
                {
                    return TenantId;
                }
                return Guid.Empty;
            }
        }
        public Guid UserId
        {
            get
            {
                if (Guid.TryParse(context.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId))
                {
                    return UserId;
                }
                return Guid.Empty;
            }
        }
        public string IpAddress { get => context.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown"; }
        public string UserAgent { get => context.HttpContext?.Request.Headers.UserAgent.ToString() ?? "Unknown"; }
    }
}
