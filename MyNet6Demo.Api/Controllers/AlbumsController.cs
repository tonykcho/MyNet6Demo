using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyNet6Demo.Api.Controllers
{
    [Route("api/[controller]")]
    public class AlbumsController : ControllerBase
    {
        [Authorize]
        public async Task<IActionResult> GetAlbumById()
        {
            await Task.Delay(10);

            return Ok("Hello");
        }
    }
}