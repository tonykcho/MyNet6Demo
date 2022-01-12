using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Domain.DomainEvents
{
    public class SongCreatedEvent : DomainEvent
    {
        public Song Song { get; set; }

        public SongCreatedEvent(Song song)
        {
            Song = song;
        }
    }

    public class SongCreatedEventHandler : DomainEventHandler<SongCreatedEvent>
    {
        private readonly ILogger<SongCreatedEventHandler> _logger;

        public SongCreatedEventHandler(ILogger<SongCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public override async Task Handle(SongCreatedEvent domainEvent)
        {
            _logger.LogInformation("--> Received Song Created!!!");

            await Task.CompletedTask;
        }
    }
}