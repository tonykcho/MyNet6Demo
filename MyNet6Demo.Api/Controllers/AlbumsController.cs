using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Api.Controllers
{
    [Route("api/[controller]")]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumsController(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        // [Authorize]
        [HttpGet("{id}", Name = "GetAlbumById")]
        public async Task<IActionResult> GetAlbumById(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var album = await _albumRepository.GetByIdAsync(id, cancellationToken);

            if (album is null)
            {
                return NotFound();
            }

            return Ok(album);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlbum()
        {
            return CreatedAtRoute("GetAlbumById", new { id = 1 }, null);
        }
    }
}