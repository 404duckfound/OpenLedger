using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLedger.Application.Commands.Auth.Login;
using OpenLedger.Application.Commands.Auth.Refresh;
using OpenLedger.Application.Commands.Auth.Register;
using OpenLedger.Application.Commands.Auth.Revoke;
using OpenLedger.Application.Commands.Auth.RevokeAll;
using OpenLedger.Application.Dtos;

namespace OpenLedger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthControllers(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] AuthRegisterCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] AuthLoginCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] AuthRefreshCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }
        [Authorize]
        [HttpPost("revoke")]
        public async Task<ActionResult> Revoke([FromBody] AuthRevokeCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }
        [Authorize]
        [HttpPost("revokeall")]
        public async Task<ActionResult> RevokeAll([FromBody] AuthRevokeAllCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }
    }
}
