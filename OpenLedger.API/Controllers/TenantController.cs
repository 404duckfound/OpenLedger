using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenLedger.Application.Commands.TenantCommands.Create;
using OpenLedger.Application.Dtos.Auth;

namespace OpenLedger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    public class TenantController(IMediator mediator) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<ActionResult<AccessTokenDto>> CreateTenant([FromBody] TenantCreateCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
