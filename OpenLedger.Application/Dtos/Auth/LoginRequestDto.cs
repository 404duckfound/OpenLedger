using System.ComponentModel.DataAnnotations;

namespace OpenLedger.Application.Dtos.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email is required."), EmailAddress(ErrorMessage = "Email is not valid.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(72, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 72 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", ErrorMessage = "Password must contain at least one uppercase, one lowercase letter, and one number.")]
        public required string Password { get; set; }
    }
}
