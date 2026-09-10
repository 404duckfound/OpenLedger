using AutoMapper;
using OpenLedger.Application.Commands.TenantCommands.Update;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Profiles
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            CreateMap<TenantUpdateCommand, Tenant>();
        }
    }
}
