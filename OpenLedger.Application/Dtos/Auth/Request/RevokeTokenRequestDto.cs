using FluentValidation;

public record RevokeTokenRequestDto(string RefreshToken);
public class RevokeTokenRequestDtoValidator : AbstractValidator<RevokeTokenRequestDto>
{
    public RevokeTokenRequestDtoValidator()
    {
        RuleFor(r => r.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .MaximumLength(100);
    }
}