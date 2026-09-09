using MediatR;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Commands.AuthCommands.Login
{
    public class AuthLoginCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, ICurrentUserService currentUser) : IRequestHandler<AuthLoginCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(AuthLoginCommand request, CancellationToken cancellationToken)
        {
            User user = await userRepository.GetByEmailAsync(request.Email, cancellationToken) ?? throw new UnauthorizedException("Invalid email or password");
            if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash)) throw new UnauthorizedException("Invalid email or password");

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var generatedRefreshToken = tokenGenerator.GenerateRefreshToken();
            var refreshToken = new RefreshToken(user.Id, generatedRefreshToken.RefreshToken, generatedRefreshToken.RefreshTokenExpires, currentUser.IpAddress, currentUser.UserAgent);

            await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(jwt, refreshToken.Token, refreshToken.ExpiresAt);
        }
    }
}
