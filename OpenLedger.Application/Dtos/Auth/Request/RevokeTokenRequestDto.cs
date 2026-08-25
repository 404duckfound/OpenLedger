using FluentValidation;

public record RefreshTokenRequestDto(string RefreshToken);
public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenRequestDtoValidator()
    {
        RuleFor(r => r.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .MaximumLength(100);
    }
}