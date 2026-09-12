using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;

namespace OpenLedger.Application.Commands.Auth
{
    public record AuthLogoutAllCommand();

    public class AuthLogoutAllCommandHandler(IRefreshTokenRepository refreshTokenRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        public async Task<string> HandleAsync(AuthLogoutAllCommand request, CancellationToken cancellationToken)
        {
            await refreshTokenRepository.RevokeAllByUserIdAsync(currentUserService.UserId, currentUserService.IpAddress, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Logged out all sessions successfully.";
        }
    }
}
