using FluentValidation;

namespace OpenLedger.Application.Dtos.Auth.Request
{
    public record RefreshTokenRequestDto(string RefreshToken, string AccessToken);
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {
            RuleFor(r => r.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MaximumLength(100);

            RuleFor(r => r.AccessToken)
                .NotEmpty().WithMessage("Access token is required.");
        }
    }
}
