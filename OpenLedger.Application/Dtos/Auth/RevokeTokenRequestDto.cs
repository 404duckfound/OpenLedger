using System.ComponentModel.DataAnnotations;

namespace OpenLedger.Application.Dtos.Auth
{
    public record RevokeTokenRequestDto
    {
        [Required(ErrorMessage = "Refresh token is required.")]
        public required string RefreshToken { get; set; }
    }
}
