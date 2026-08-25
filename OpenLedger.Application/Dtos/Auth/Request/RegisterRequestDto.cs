using FluentValidation;
using System.Text.RegularExpressions;

namespace OpenLedger.Application.Dtos.Auth.Request
{
    public record RegisterRequestDto(string Email, string Name, string Password, string ConfirmPassword);

    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        private static readonly Regex NameRegex = new(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", RegexOptions.Compiled);
        private static readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", RegexOptions.Compiled);
        public RegisterRequestDtoValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Matches(NameRegex).WithMessage("Name can only contain letters and spaces.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(PasswordRegex).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.")
                .Length(8, 72).WithMessage("Password must be between 8 and 72 characters long.");

            RuleFor(r => r.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(r => r.Password).WithMessage("Passwords do not match.");
        }
    }
}
