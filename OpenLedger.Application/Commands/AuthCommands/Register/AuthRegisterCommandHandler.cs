using MediatR;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Commands.AuthCommands.Register
{
    public class AuthRegisterCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, ICurrentUserService currentUser) : IRequestHandler<AuthRegisterCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(AuthRegisterCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = passwordHasher.HashPassword(request.Password);
            var user = new User(request.Name, request.Email, passwordHash);
            var jwt = tokenGenerator.GenerateJwtToken(user);
            var createRefreshToken = tokenGenerator.GenerateRefreshToken();
            var refreshToken = new RefreshToken(user.Id, createRefreshToken.RefreshToken, createRefreshToken.RefreshTokenExpires, currentUser.IpAddress, currentUser.UserAgent);

            await userRepository.AddAsync(user, cancellationToken);
            await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(jwt, refreshToken.Token, refreshToken.ExpiresAt);
        }
    }
}
