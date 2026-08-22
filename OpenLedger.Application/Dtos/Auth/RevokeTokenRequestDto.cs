using System.ComponentModel.DataAnnotations;

namespace OpenLedger.Application.Dtos.Auth
{
    public class RevokeTokenRequestDto
    {
        [Required(ErrorMessage = "Refresh token is required.")]
        public required string RefreshToken { get; set; }
    }
}
