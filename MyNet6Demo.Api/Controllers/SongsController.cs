using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNet6Demo.Core.Songs.Commands;
using MyNet6Demo.Core.Songs.Queries;

namespace MyNet6Demo.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SongsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("{guid}", Name = "GetSongByGuidAsync")]
        public async Task<IActionResult> GetSongByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var song = await _mediator.Send(new GetSongByGuidQuery { Guid = guid }, cancellationToken);

            return Ok(song);
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetSongListAsync")]
        public async Task<IActionResult> GetSongListAsync([FromQuery] GetSongListQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("csv", Name = "ExportSongListAsync")]
        public async Task<IActionResult> ExportSongListAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var csv = await _mediator.Send(new ExportSongListQuery());

            return File(csv.Content, csv.ContentType, csv.FileName);
        }

        [HttpPost(Name = "CreateSongAsync")]
        public async Task<IActionResult> CreateSongAsync([FromBody] CreateSongCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var song = await _mediator.Send(command, cancellationToken);

            return CreatedAtRoute("GetSongByGuidAsync", new { Guid = song.Guid }, song);
        }

        [HttpPut("{guid}", Name = "UpdateSongAsync")]
        public async Task<IActionResult> UpdateSongAsync(Guid guid, [FromBody] UpdateSongCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (guid != command.Guid)
            {
                return BadRequest();
            }

            var song = await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{guid}", Name = "DeleteSongAsync")]
        public async Task<IActionResult> DeleteSongAsync(Guid guid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _mediator.Send(new DeleteSongCommand { Guid = guid });

            return NoContent();
        }
    }
}