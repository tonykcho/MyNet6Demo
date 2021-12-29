using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Domain.DomainEvents
{
    public class AlbumDeletedEvent : DomainEvent
    {
        public Album Album { get; set; }

        public AlbumDeletedEvent(Album album)
        {
            Album = album;
        }
    }

    public class AlbumDeletedEventHandler : DomainEventHandler<AlbumDeletedEvent>
    {
        private readonly ILogger<AlbumDeletedEventHandler> _logger;

        public AlbumDeletedEventHandler(ILogger<AlbumDeletedEventHandler> logger)
        {
            _logger = logger;
        }

        public override async Task Handle(AlbumDeletedEvent domainEvent)
        {
            _logger.LogInformation("--> Received Album Deleted");
        }
    }
}