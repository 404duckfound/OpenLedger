using OpenLedger.Application.Commands.Tenant;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Mappers
{
    public static class TenantMapperExtensions
    {
        public static void UpdateRequestTo(this TenantUpdateCommand command, Tenant tenant)
        {
            tenant.Update(
               name: command.Name,
               email: command.Email,
               taxNumber: command.TaxNumber,
               taxOffice: command.TaxOffice,
               phoneNumber: command.PhoneNumber,
               address: command.Address
            );
        }
    }
}
