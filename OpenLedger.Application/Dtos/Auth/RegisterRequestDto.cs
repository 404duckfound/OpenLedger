using System.ComponentModel.DataAnnotations;

namespace OpenLedger.Application.Dtos.Auth
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Password must be between 2 and 50 characters.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage ="Name is not valid.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(72, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 72 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", ErrorMessage = "Password must contain at least one uppercase, one lowercase letter, and one number.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required."), Compare("Password", ErrorMessage = "Passwords do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
