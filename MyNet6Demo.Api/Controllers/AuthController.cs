using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MyNet6Demo.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("sign_in", Name = "LoginAsync")]
        public async Task<IActionResult> LoginAsync(CancellationToken cancellationToken)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity("MyNet6CoreDemo");

            await _httpContextAccessor.HttpContext.SignInAsync("MyNet6CoreDemo", new ClaimsPrincipal(claimsIdentity));

            return Ok();
        }

        [HttpPost("sign_out", Name = "LogoutAsync")]
        public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync("MyNet6CoreDemo");

            return NoContent();
        }
    }
}