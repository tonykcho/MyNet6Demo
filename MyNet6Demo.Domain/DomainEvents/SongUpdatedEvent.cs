using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Abstracts;

namespace MyNet6Demo.Domain.DomainEvents
{
    public class SongUpdatedEvent : DomainEvent
    {
        public SongUpdatedEvent()
        {

        }
    }

    public class SongUpdatedEventHandler : DomainEventHandler<SongUpdatedEvent>
    {
        private readonly ILogger<SongUpdatedEventHandler> _logger;

        public SongUpdatedEventHandler(ILogger<SongUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public override async Task Handle(SongUpdatedEvent domainEvent)
        {
            _logger.LogInformation("--> Received song updated");

            await Task.CompletedTask;
        }
    }
}