using System.Security.Claims;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Interfaces.Singletons
{
    public interface ITokenGenerator
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetClaimsFromJwt(string token);
    }
}
