using FluentValidation;

namespace OpenLedger.Application.Commands.Auth.Revoke
{
    public class RefreshCommandValidator : AbstractValidator<AuthRevokeCommand>
    {
        public RefreshCommandValidator()
        {
            RuleFor(r => r.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MaximumLength(100);
        }
    }
}
