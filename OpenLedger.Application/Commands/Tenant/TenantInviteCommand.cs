using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Domain.Enums;

namespace OpenLedger.Application.Commands.Tenant
{
    public record TenantInviteCommand();

    public class TenantInviteCommandHandler(ICurrentUserService currentUserService, IUserRepository userRepository, IUnitOfWork unitOfWork, ITenantRepository tenantRepository, ITokenGeneratorService tokenGenerator)
    {
        public async Task<AccessTokenDto> HandleAsync(TenantCreateCommand request, CancellationToken cancellationToken)
        {
            
        }
    }
}
