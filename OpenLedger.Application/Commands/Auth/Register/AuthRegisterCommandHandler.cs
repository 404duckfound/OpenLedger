using MediatR;
using Microsoft.Extensions.Options;
using OpenLedger.Application.Dtos;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Application.Options;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Commands.Auth.Register
{
    public class AuthRegisterCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, ICurrentUserService currentUser, IOptions<TokenOptions> options) : IRequestHandler<AuthRegisterCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(AuthRegisterCommand request, CancellationToken cancellationToken)
        {
            if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken)) throw new InvalidOperationException("Email is already exists.");

            var passwordHash = passwordHasher.HashPassword(request.Password);
            var user = new User(request.Name, request.Email, passwordHash);
            var jwt = tokenGenerator.GenerateJwtToken(user);
            var refreshToken = new RefreshToken(user.Id, tokenGenerator.GenerateRefreshToken(), DateTime.UtcNow.AddDays(options.Value.RefreshExpiresDays), currentUser.IpAddress, currentUser.UserAgent);

            await userRepository.AddAsync(user, cancellationToken);
            await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(jwt, refreshToken.Token!, refreshToken.ExpiresAt);
        }
    }
}
