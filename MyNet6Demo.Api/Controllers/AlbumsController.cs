using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyNet6Demo.Api.Controllers
{
    [Route("api/[controller]")]
    public class AlbumsController : ControllerBase
    {
        // [Authorize]
        [HttpGet("{id}", Name = "GetAlbumById")]
        public async Task<IActionResult> GetAlbumById(int id, CancellationToken cancellationToken)
        {
            // ArgumentNullException.ThrowIfNull(null);

            await Task.Delay(1);

            cancellationToken.ThrowIfCancellationRequested();

            return Ok("Hello");
        }
    }
}