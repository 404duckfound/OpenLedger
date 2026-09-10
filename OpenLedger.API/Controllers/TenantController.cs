using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLedger.Application.Commands.TenantCommands.Create;
using OpenLedger.Application.Commands.TenantCommands.Update;
using OpenLedger.Application.Dtos.Auth;

namespace OpenLedger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Authorize]
    public class TenantController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<AccessTokenDto>> Create([FromBody] TenantCreateCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> Update([FromBody] TenantUpdateCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }
    }
}
