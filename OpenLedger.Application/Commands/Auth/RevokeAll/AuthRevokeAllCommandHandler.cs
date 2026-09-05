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
            await refreshTokenRepository.DeleteAllByUserIdAsync(currentUserService.UserId, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
