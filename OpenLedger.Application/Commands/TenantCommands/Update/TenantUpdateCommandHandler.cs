using AutoMapper;
using MediatR;
using OpenLedger.API.Middlewares;
using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;

namespace OpenLedger.Application.Commands.TenantCommands.Update
{
    public class TenantUpdateCommandHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<TenantUpdateCommand, string>
    {
        public async Task<string> Handle(TenantUpdateCommand request, CancellationToken cancellationToken)
        {
            var tenant = await tenantRepository.GetByIdAsync(currentUserService.TenantId, cancellationToken) ?? throw new NotFoundException("Tenant not found.");

            var newTenant = mapper.Map(request, tenant);

            tenantRepository.Update(tenant);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Tenant updated successfully";
        }
    }
}
