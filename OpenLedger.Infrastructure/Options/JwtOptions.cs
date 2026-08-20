using System.ComponentModel.DataAnnotations;

namespace OpenLedger.Infrastructure.Options
{
    public class JwtOptions
    {
        [Required(ErrorMessage = "Jwt:Secret is required.")]
        public required string Secret { get; set; }
        [Required(ErrorMessage = "Jwt:Issuer is required.")]
        public required string Issuer { get; set; }
        [Required(ErrorMessage = "Jwt:Audience is required.")]
        public required string Audience { get; set; }
        [Required(ErrorMessage = "Jwt:Expires is required."), Range(1, int.MaxValue, ErrorMessage = "Jwt:Expires must be a positive integer.")]
        public required int Expires { get; set; }
    }
}
