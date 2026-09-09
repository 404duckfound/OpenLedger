using MediatR;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;

namespace OpenLedger.Application.Commands.AuthCommands.LogoutAll
{
    public class AuthLogoutAllCommandHandler(IRefreshTokenRepository refreshTokenRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork) : IRequestHandler<AuthLogoutAllCommand, string>
    {
        public async Task<string> Handle(AuthLogoutAllCommand request, CancellationToken cancellationToken)
        {
            await refreshTokenRepository.RevokeAllByUserIdAsync(currentUserService.UserId, currentUserService.IpAddress, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Logged out all sessions successfully.";
        }
    }
}
