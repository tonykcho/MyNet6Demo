using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Domain.DomainEvents
{
    public class ArtistDeletedEvent : DomainEvent
    {
        public Artist Artist { get; set; }

        public ArtistDeletedEvent(Artist artist)
        {
            Artist = artist;
        }
    }

    public class ArtistDeletedEventHandler : DomainEventHandler<ArtistDeletedEvent>
    {
        private readonly ILogger<ArtistDeletedEvent> _logger;

        public ArtistDeletedEventHandler(ILogger<ArtistDeletedEvent> logger)
        {
            _logger = logger;
        }

        public override async Task Handle(ArtistDeletedEvent domainEvent)
        {
            _logger.LogInformation("--> Received Artist Deleted");
        }
    }
}