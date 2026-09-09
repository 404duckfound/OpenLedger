using FluentValidation;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using System.Text.RegularExpressions;

namespace OpenLedger.Application.Commands.AuthCommands.Register
{
    public class AuthRegisterCommandValidator : AbstractValidator<AuthRegisterCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", RegexOptions.Compiled);
        public AuthRegisterCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .MaximumLength(100).WithMessage("Email address must not exceed 100 characters.")
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .MustAsync(IsUniqueEmail).WithMessage("Email address is already in use.");

            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(PasswordRegex).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.")
                .Length(8, 72).WithMessage("Password must be between 8 and 72 characters long.");

            RuleFor(r => r.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(r => r.Password).WithMessage("Passwords do not match.");
        }
        public async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return !await _userRepository.ExistsByEmailAsync(email, cancellationToken);
        }
    }
}
