using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNet6Demo.Core.Albums.Commands;
using MyNet6Demo.Core.Albums.Queries;

namespace MyNet6Demo.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AlbumsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlbumsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("{guid}", Name = "GetAlbumByGuidAsync")]
        public async Task<IActionResult> GetAlbumByGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var album = await _mediator.Send(new GetAlbumByGuidQuery { Guid = guid }, cancellationToken);

            return Ok(album);
        }

        [AllowAnonymous]
        [HttpGet("{guid}/image", Name = "GetAlbumImageByGuidAsync")]
        public async Task<IActionResult> GetAlbumImageByGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var imageContent = await _mediator.Send(new GetAlbumImageByGuidQuery { Guid = guid }, cancellationToken);

            return File(imageContent.Image, imageContent.ContentType);
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetAlbumListAsync")]
        public async Task<IActionResult> GetAlbumListAsync([FromQuery] GetAlbumListQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("csv", Name = "ExportAlbumListAsync")]
        public async Task<IActionResult> ExportAlbumListAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var csv = await _mediator.Send(new ExportAlbumListQuery());

            return File(csv.Content, csv.ContentType, csv.FileName);
        }

        [HttpPost(Name = "CreateAlbumAsync")]
        public async Task<IActionResult> CreateAlbumAsync([FromForm] CreateAlbumCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var album = await _mediator.Send(command, cancellationToken);

            return CreatedAtRoute("GetAlbumByGuidAsync", new { guid = album.Guid }, album);
        }

        [HttpPut("{guid}", Name = "UpdateAlbumAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateAlbumAsync([FromRoute] Guid guid, [FromBody] UpdateAlbumCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (guid != command.Guid)
            {
                return BadRequest();
            }

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{guid}/image", Name = "UpdateAlbumImageAsync")]
        public async Task<IActionResult> UpdateAlbumImageAsync([FromRoute] Guid guid, [FromForm] UploadAlbumImageCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (guid != command.Guid)
            {
                return BadRequest();
            }

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{guid}", Name = "DeleteAlbumAsync")]
        public async Task<IActionResult> DeleteAlbumAsync([FromRoute] Guid guid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _mediator.Send(new DeleteAlbumCommand { Guid = guid });

            return NoContent();
        }
    }
}