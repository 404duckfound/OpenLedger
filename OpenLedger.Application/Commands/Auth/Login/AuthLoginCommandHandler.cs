using MediatR;
using Microsoft.Extensions.Options;
using OpenLedger.Application.Dtos;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Application.Options;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Commands.Auth.Login
{
    public class AuthLoginCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, ICurrentUserService currentUser, IOptions<TokenOptions> options) : IRequestHandler<AuthLoginCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(AuthLoginCommand request, CancellationToken cancellationToken)
        {
            User user = await userRepository.GetByEmailAsync(request.Email, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid email or password");
            if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash)) throw new UnauthorizedAccessException("Invalid email or password");

            var jwt = tokenGenerator.GenerateJwtToken(user);
            var refreshToken = new RefreshToken(user.Id, tokenGenerator.GenerateRefreshToken(), DateTime.UtcNow.AddDays(options.Value.RefreshExpiresDays), currentUser.IpAddress, currentUser.UserAgent);

            await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(jwt, refreshToken.Token!, refreshToken.ExpiresAt);
        }
    }
}
