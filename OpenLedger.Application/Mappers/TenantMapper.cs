using OpenLedger.Application.Commands.Tenant;
using OpenLedger.Domain.Entities.Auth;
using Riok.Mapperly.Abstractions;

namespace OpenLedger.Application.Mappers
{
    [Mapper]
    public static partial class TenantMapper
    {
        public static partial void UpdateRequestTo(this TenantUpdateCommand command, Tenant tenant);
    }
}
