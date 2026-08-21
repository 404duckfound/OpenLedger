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
    public class TokenGenerator(IOptions<JwtOptions> jwtOptions) : ITokenGenerator
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));

            var tokenDescriptor = new JwtSecurityToken(
                issuer: jwtOptions.Value.Issuer,
                audience: jwtOptions.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtOptions.Value.Expires),
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
