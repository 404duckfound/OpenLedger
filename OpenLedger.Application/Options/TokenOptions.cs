using System.ComponentModel.DataAnnotations;

namespace OpenLedger.Application.Options
{
    public class TokenOptions
    {
        [Required(ErrorMessage = "Token:JwtSecret is required.")]
        public required string JwtSecret { get; set; }
        [Required(ErrorMessage = "Token:JwtIssuer is required.")]
        public required string JwtIssuer { get; set; }
        [Required(ErrorMessage = "Token:JwtAudience is required.")]
        public required string JwtAudience { get; set; }
        [Required(ErrorMessage = "Token:JwtExpires is required.")]
        public required int JwtExpires { get; set; }

        [Required(ErrorMessage = "Token:RefreshExpiresDays is required.")]
        public required int RefreshExpiresDays { get; set; }
    }
}
