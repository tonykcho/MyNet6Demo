using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Abstracts;

namespace MyNet6Demo.Domain.DomainEvents
{
    public class ArtistUpdatedEvent : DomainEvent
    {
        public ArtistUpdatedEvent()
        {

        }
    }

    public class ArtistUpdatedEventHandler : DomainEventHandler<ArtistUpdatedEvent>
    {
        private readonly ILogger<ArtistUpdatedEventHandler> _logger;

        public ArtistUpdatedEventHandler(ILogger<ArtistUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public override async Task Handle(ArtistUpdatedEvent domainEvent)
        {
            _logger.LogInformation("--> Received Artist Updated Event");

            await Task.CompletedTask;
        }
    }
}