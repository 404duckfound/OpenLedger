using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLedger.Application.Commands.Tenant;
using OpenLedger.Application.Dtos.Auth;
using Wolverine;

namespace OpenLedger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Authorize]
    public class TenantController(IMessageBus bus) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<AccessTokenDto>> Create([FromBody] TenantCreateCommand command)
        {
            var result = await bus.InvokeAsync<AccessTokenDto>(command);
            return Ok(result);
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> Update([FromBody] TenantUpdateCommand command)
        {
            var res = await bus.InvokeAsync<string>(command);
            return Ok(res);
        }
    }
}
