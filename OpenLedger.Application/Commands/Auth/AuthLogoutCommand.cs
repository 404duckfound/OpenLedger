using FluentValidation;
using MediatR;
using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;

namespace OpenLedger.Application.Commands.Auth
{
    public record AuthLogoutCommand(string RefreshToken) : IRequest<string>;

    public class AuthLogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<AuthLogoutCommand, string>
    {
        public async Task<string> Handle(AuthLogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

            if (refreshToken is null || refreshToken.UserId != currentUser.UserId) throw new BadRequestException("Invalid refresh token.");

            refreshToken.Revoke(currentUser.IpAddress);
            refreshTokenRepository.Update(refreshToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Logged out successfully.";
        }
    }

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