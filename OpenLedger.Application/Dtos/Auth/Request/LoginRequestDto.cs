using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OpenLedger.Application.Dtos.Auth.Request
{
    public record LoginRequestDto(string Email, string Password);
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        private static readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", RegexOptions.Compiled);
        public LoginRequestDtoValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(PasswordRegex).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.")
                .Length(8, 72).WithMessage("Password must be between 8 and 72 characters long.");
        }
    }
}
