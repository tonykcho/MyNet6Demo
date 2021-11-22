using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MyNet6Demo.Api.Controllers
{
    public class SongsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SongsController(IMediator mediator)
        {
            _mediator = mediator;
        }

    }
}