using MediatR;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;

namespace OpenLedger.Application.Commands.Auth.RevokeAll
{
    public class AuthRevokeAllCommandHandler(IRefreshTokenRepository refreshTokenRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork) : IRequestHandler<AuthRevokeAllCommand>
    {
        public async Task Handle(AuthRevokeAllCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            await refreshTokenRepository.DeleteAllByUserIdAsync(userId, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            return;
        }
    }
}
