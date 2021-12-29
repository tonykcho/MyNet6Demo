using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Domain.DomainEvents
{
    public class SongDeletedEvent : DomainEvent
    {
        public Song Song { get; set; }

        public SongDeletedEvent(Song song)
        {
            Song = song;
        }
    }

    public class SongDeletedEventHandler : DomainEventHandler<SongDeletedEvent>
    {
        private readonly ILogger<SongDeletedEventHandler> _logger;

        public SongDeletedEventHandler(ILogger<SongDeletedEventHandler> logger)
        {
            _logger = logger;
        }

        public override async Task Handle(SongDeletedEvent domainEvent)
        {
            _logger.LogInformation("--> Received Song Deleted");
        }
    }
}