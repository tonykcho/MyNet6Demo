using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNet6Demo.Core.Albums.Commands;
using MyNet6Demo.Core.Albums.Queries;

namespace MyNet6Demo.Api.Controllers
{
    [Route("api/[controller]")]
    public class AlbumsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlbumsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetAlbumListAsync")]
        public async Task<IActionResult> GetAlbumListAsync([FromQuery] GetAlbumListQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("export", Name = "ExportAlbumListAsync")]
        public async Task<IActionResult> ExportAlbumListAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var csv = await _mediator.Send(new ExportAlbumListQuery());

            return File(csv.Content, csv.ContentType, csv.FileName);
        }

        // [Authorize]
        [HttpGet("{guid}", Name = "GetAlbumByGuidAsync")]
        public async Task<IActionResult> GetAlbumByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var album = await _mediator.Send(new GetAlbumByGuidQuery { Guid = guid }, cancellationToken);

            return Ok(album);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> CreateAlbumAsync([FromBody] CreateAlbumCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var album = await _mediator.Send(command, cancellationToken);

            return CreatedAtRoute("GetAlbumByGuidAsync", new { guid = album.Guid }, album);
        }

        [HttpPut("{guid}", Name = "UpdateAlbumAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateAlbumAsync(Guid guid, [FromBody] UpdateAlbumCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if(guid != command.Guid)
            {
                return BadRequest();
            }

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{guid}", Name = "DeleteAlbumAsync")]
        public async Task<IActionResult> DeleteAlbumAsync(Guid guid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _mediator.Send(new DeleteAlbumCommand { Guid = guid });

            return NoContent();
        }
    }
}