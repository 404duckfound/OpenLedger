using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenLedger.Application.Interfaces.Auth;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OpenLedger.Infrastructure.Singletons
{
    public class TokenGenerator(IOptions<TokenOptions> tokenOptions) : ITokenGenerator
    {
        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Name, user.Name),
                new(ClaimTypes.Role, user.Role.ToString()),
            };

            if (user.TenantId != Guid.Empty) claims.Add(new("TenantId",user.TenantId.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Value.JwtSecret));

            var tokenDescriptor = new JwtSecurityToken(
                issuer: tokenOptions.Value.JwtIssuer,
                audience: tokenOptions.Value.JwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenOptions.Value.JwtExpires),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
