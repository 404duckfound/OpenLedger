using System.ComponentModel.DataAnnotations;

namespace OpenLedger.Application.Options
{
    public class TokenOptions
    {
        public const string SectionName = "TokenOptions";

        [Required(ErrorMessage = "JwtSecret is required.")]
        public string? JwtSecret { get; set; }
        [Required(ErrorMessage = "JwtIssuer is required.")]
        public string? JwtIssuer { get; set; }
        [Required(ErrorMessage = "JwtAudience is required.")]
        public string? JwtAudience { get; set; }
        [Required(ErrorMessage = "JwtExpires is required.")]
        public int JwtExpires { get; set; }
        [Required(ErrorMessage = "RefreshExpiresDays is required.")]
        public int RefreshExpiresDays { get; set; }
    }
}
