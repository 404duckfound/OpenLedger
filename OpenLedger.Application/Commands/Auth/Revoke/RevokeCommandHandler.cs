using MediatR;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;

namespace OpenLedger.Application.Commands.Auth.Revoke
{
    public class RevokeCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<RevokeCommand>
    {
        public async Task Handle(RevokeCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid refresh token.");
            
            if (refreshToken.UserId != currentUser.UserId) throw new UnauthorizedAccessException("Wrong user.");

            refreshToken.Revoke(currentUser.IpAddress, "Revoked.");

            await refreshTokenRepository.Update(refreshToken, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return;
        }
    }
}
