using FluentValidation;

namespace OpenLedger.Application.Commands.Auth.Refresh
{
    public class AuthRefreshCommandValidator : AbstractValidator<AuthRefreshCommand>
    {
        public AuthRefreshCommandValidator()
        {
            RuleFor(r => r.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MaximumLength(100);

            RuleFor(r => r.AccessToken)
                .NotEmpty().WithMessage("Access token is required.");
        }
    }
}
