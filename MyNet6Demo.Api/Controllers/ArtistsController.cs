using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyNet6Demo.Core.Artists.Commands;
using MyNet6Demo.Core.Artists.Queries;

namespace MyNet6Demo.Api.Controllers
{
    [Route("api/[controller]")]
    public class ArtistsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArtistsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{guid}", Name = "GetArtistByGuidAsync")]
        public async Task<IActionResult> GetArtistByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var artist = await _mediator.Send(new GetArtistByGuidQuery { Guid = guid });

            return Ok(artist);
        }

        [HttpGet(Name = "GetArtistListAsync")]
        public async Task<IActionResult> GetArtistListAsync([FromQuery] GetArtistListQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var view = await _mediator.Send(query, cancellationToken);

            return Ok(view);
        }

        [HttpPost(Name = "CreateArtistAsync")]
        public async Task<IActionResult> CreateArtistAsync([FromBody] CreateArtistCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var view = await _mediator.Send(command);

            return CreatedAtRoute("GetArtistByGuidAsync", new { guid = view.Guid }, view);
        }

        [HttpPut("{guid}", Name = "UpdateArtistAsync")]
        public async Task<IActionResult> UpdateArtistAsync(Guid guid, [FromBody] UpdateArtistCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if(guid != command.Guid)
            {
                return BadRequest();
            }
            
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{guid}", Name = "DeleteArtistAsync")]
        public async Task<IActionResult> DeleteArtistAsync(Guid guid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _mediator.Send(new DeleteArtistCommand { Guid = guid });

            return NoContent();
        }
    }
}