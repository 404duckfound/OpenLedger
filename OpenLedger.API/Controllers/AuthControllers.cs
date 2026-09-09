using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLedger.Application.Commands.AuthCommands.Login;
using OpenLedger.Application.Commands.AuthCommands.Logout;
using OpenLedger.Application.Commands.AuthCommands.LogoutAll;
using OpenLedger.Application.Commands.AuthCommands.Refresh;
using OpenLedger.Application.Commands.AuthCommands.Register;
using OpenLedger.Application.Dtos.Auth;

namespace OpenLedger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    public class AuthController(IMediator mediator) : ControllerBase
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
        [HttpPost("logout")]
        public async Task<ActionResult<string>> Logout([FromBody] AuthLogoutCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }
        [Authorize]
        [HttpPost("logout-all")]
        public async Task<ActionResult<string>> LogoutAll([FromBody] AuthLogoutAllCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }
    }
}
