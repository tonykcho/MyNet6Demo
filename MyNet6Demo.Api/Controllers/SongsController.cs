using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyNet6Demo.Core.Songs.Commands;
using MyNet6Demo.Core.Songs.Queries;

namespace MyNet6Demo.Api.Controllers
{
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SongsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{guid}", Name = "GetSongByGuidAsync")]
        public async Task<IActionResult> GetSongByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var song = await _mediator.Send(new GetSongByGuidQuery { Guid = guid }, cancellationToken);

            return Ok(song);
        }

        [HttpPost(Name = "CreateSongAsync")]
        public async Task<IActionResult> CreateSongAsync([FromBody] CreateSongCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var song = await _mediator.Send(command, cancellationToken);

            return CreatedAtRoute("GetSongByGuidAsync", new { Guid = song.Guid }, song);
        }
    }
}