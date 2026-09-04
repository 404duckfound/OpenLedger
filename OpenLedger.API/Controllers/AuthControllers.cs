using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLedger.Application.Commands.Auth.Login;
using OpenLedger.Application.Commands.Auth.Refresh;
using OpenLedger.Application.Commands.Auth.Register;
using OpenLedger.Application.Commands.Auth.Revoke;
using OpenLedger.Application.Dtos;

namespace OpenLedger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthControllers(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }
        [Authorize]
        [HttpPost("revoke")]
        public async Task<ActionResult> Revoke([FromBody] RevokeCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }
    }
}
