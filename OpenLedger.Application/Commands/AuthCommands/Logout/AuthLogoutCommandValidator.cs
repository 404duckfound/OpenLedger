using FluentValidation;

namespace OpenLedger.Application.Commands.AuthCommands.Logout
{
    public class AuthLogoutCommandValidator : AbstractValidator<AuthLogoutCommand>
    {
        public AuthLogoutCommandValidator()
        {
            RuleFor(r => r.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MaximumLength(100).WithMessage("Refresh token is too long.");
        }
    }
}
