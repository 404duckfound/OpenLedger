using System.ComponentModel.DataAnnotations;

namespace OpenLedger.Application.Dtos.Auth
{
    public record RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "Refresh token is required.")]
        public required string RefreshToken { get; set; }
        [Required(ErrorMessage = "Access token is required.")]
        public required string AccessToken { get; set; }
    }
}
