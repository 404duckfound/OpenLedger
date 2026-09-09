using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Domain.Constants;
using System.Security.Claims;

namespace OpenLedger.API.Services
{
    public class CurrentUserService(IHttpContextAccessor context) : ICurrentUserService
    {
        public Guid TenantId
        {
            get
            {
                if (Guid.TryParse(context.HttpContext?.User.FindFirstValue(ApplicationClaims.TenantId), out Guid TenantId))
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
                if (Guid.TryParse(context.HttpContext?.User.FindFirstValue(ApplicationClaims.UserId), out Guid UserId))
                {
                    return UserId;
                }
                throw new UnauthorizedException("User is not authenticated.");
            }
        }
        public string IpAddress { get => context.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown"; }
        public string UserAgent { get => context.HttpContext?.Request.Headers.UserAgent.ToString() ?? "Unknown"; }
    }
}
