using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNet6Demo.Core.Albums.Commands;
using MyNet6Demo.Core.Albums.Queries;
using MyNet6Demo.Domain.Exceptions;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;

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

        // [Authorize]
        [HttpGet("{guid}", Name = "GetAlbumByGuidAsync")]
        public async Task<IActionResult> GetAlbumByGuidAsync([FromRoute] GetAlbumByGuidQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var album = await _mediator.Send(query, cancellationToken);

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
    }
}