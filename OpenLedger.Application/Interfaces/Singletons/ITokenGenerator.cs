using System.Security.Claims;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Interfaces.Singletons
{
    public interface ITokenGenerator
    {
        string GenerateJwtToken(User user);
        RefreshTokenDto GenerateRefreshToken();
        ClaimsPrincipal GetClaimsFromJwt(string token);
    }
}
