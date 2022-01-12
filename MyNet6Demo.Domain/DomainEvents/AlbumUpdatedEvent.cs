using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Domain.DomainEvents
{
    public class AlbumUpdatedEvent : DomainEvent
    {
        public Album Album { get; set; }
        public AlbumUpdatedEvent(Album album)
        {
            Album = album;
        }
    }

    public class AlbumUpdatedEventHandler : IDomainEventHandler<AlbumUpdatedEvent>
    {
        private readonly ILogger<AlbumUpdatedEventHandler> _logger;

        public AlbumUpdatedEventHandler(ILogger<AlbumUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(AlbumUpdatedEvent domainEvent)
        {
            _logger.LogInformation("--> Received album updated!!!");
            
            await Task.CompletedTask;
        }
    }
}