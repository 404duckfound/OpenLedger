using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Application.Options;
using OpenLedger.Domain.Entities.Auth;
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

            if (user.TenantId != Guid.Empty) claims.Add(new("TenantId", user.TenantId.ToString()));

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
        public ClaimsPrincipal GetClaimsFromJwt(string token)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Value.JwtSecret)),
                ValidateLifetime = false
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token algorithm.");
            }

            return principal;
        }
    }
}
