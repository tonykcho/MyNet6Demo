using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Domain.DomainEvents
{
    public class AlbumCreatedEvent : DomainEvent
    {
        public Album Album { get; set; }
        public AlbumCreatedEvent(Album album)
        {
            Album = album;
        }
    }

    public class AlbumCreatedEventHandler : DomainEventHandler<AlbumCreatedEvent>
    {
        private readonly ILogger<AlbumCreatedEventHandler> _logger;

        public AlbumCreatedEventHandler(ILogger<AlbumCreatedEventHandler> logger)
        {
            _logger = logger;
        }
        public override async Task Handle(AlbumCreatedEvent domainEvent)
        {
            _logger.LogInformation("--> Received album created!!!");
        }
    }
}