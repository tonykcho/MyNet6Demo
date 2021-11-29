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

        [HttpPost]
        public async Task<IActionResult> CreateArtistAsync(CreateArtistCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var view = await _mediator.Send(command);

            return CreatedAtRoute("", new { guid = view.Guid }, view);
        }
    }
}