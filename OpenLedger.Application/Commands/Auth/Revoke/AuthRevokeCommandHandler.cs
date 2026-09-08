using MediatR;
using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;

namespace OpenLedger.Application.Commands.Auth.Revoke
{
    public class AuthRevokeCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<AuthRevokeCommand>
    {
        public async Task Handle(AuthRevokeCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

            if (refreshToken is null || refreshToken.UserId != currentUser.UserId) throw new BadRequestException("Invalid refresh token.");

            refreshToken.Revoke(currentUser.IpAddress);
            refreshTokenRepository.Update(refreshToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
