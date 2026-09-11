using Mapster;
using MediatR;
using OpenLedger.API.Middlewares;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;

namespace OpenLedger.Application.Commands.TenantCommands.Update
{
    public class TenantUpdateCommandHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<TenantUpdateCommand, string>
    {
        public async Task<string> Handle(TenantUpdateCommand request, CancellationToken cancellationToken)
        {
            var tenant = await tenantRepository.GetByIdAsync(currentUserService.TenantId, cancellationToken) ?? throw new NotFoundException("Tenant not found.");

            request.Adapt(tenant);

            tenantRepository.Update(tenant);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Tenant updated successfully";
        }
    }
}
