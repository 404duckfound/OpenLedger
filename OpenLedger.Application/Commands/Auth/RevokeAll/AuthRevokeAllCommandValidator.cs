using FluentValidation;

namespace OpenLedger.Application.Commands.Auth.RevokeAll
{
    public class AuthRevokeAllCommandValidator : AbstractValidator<AuthRevokeAllCommand>
    {
        public AuthRevokeAllCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(userId => userId != Guid.Empty).WithMessage("UserId cannot be an empty GUID.");
        }
    }
}
