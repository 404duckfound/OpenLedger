using System.Security.Claims;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Interfaces.Services
{
    public interface ITokenGeneratorService
    {
        string GenerateJwtToken(User user);
        RefreshTokenDto GenerateRefreshToken();
        ClaimsPrincipal GetClaimsFromJwt(string token);
    }
}
