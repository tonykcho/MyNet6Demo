using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Domain.DomainEvents
{
    public class ArtistCreatedEvent : DomainEvent
    {
        public Artist Artist { get; set; }

        public ArtistCreatedEvent(Artist artist)
        {
            Artist = artist;
        }
    }

    public class ArtistCreatedEventHandler : DomainEventHandler<ArtistCreatedEvent>
    {
        private readonly ILogger<ArtistCreatedEventHandler> _logger;

        public ArtistCreatedEventHandler(ILogger<ArtistCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public override async Task Handle(ArtistCreatedEvent domainEvent)
        {
            _logger.LogInformation($"--> Received artist created event: {domainEvent.Artist.Name}");

            await Task.CompletedTask;
        }
    }
}