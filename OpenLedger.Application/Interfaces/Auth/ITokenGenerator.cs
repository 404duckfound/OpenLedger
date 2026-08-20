using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Interfaces.Auth
{
    public interface ITokenGenerator
    {
       public string GenerateJwtToken(User user);
       public string GenerateRefreshToken();
    }
}
