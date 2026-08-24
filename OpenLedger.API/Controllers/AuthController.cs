using Microsoft.AspNetCore.Mvc;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Interfaces.Services;

namespace OpenLedger.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto loginRequest, CancellationToken cancellationToken = default)
        {
            return Ok(await authService.LoginAsync(loginRequest, cancellationToken));
        }
    }
}
