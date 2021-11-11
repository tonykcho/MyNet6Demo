using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyNet6Demo.Api.Controllers
{
    [Route("api/[controller]")]
    public class AlbumsController : ControllerBase
    {
        // [Authorize]
        public async Task<IActionResult> GetAlbumById(CancellationToken cancellationToken)
        {
            await Task.Delay(10000);

            cancellationToken.ThrowIfCancellationRequested();

            return Ok("Hello");
        }
    }
}