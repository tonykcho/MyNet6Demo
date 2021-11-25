using MyNet6Demo.Domain.Interfaces;
using System.Text.Json;

namespace MyNet6Demo.Core.Services
{
    public class DomainEventProcessor : IDomainEventProcessor
    {
        public IDictionary<string, Type> DomainEventMap { get; set; }
        public IDictionary<string, Type> DomainEventHandlers { get; set; }

        private readonly IServiceProvider _serviceProvider;

        public DomainEventProcessor(IServiceProvider serviceProvider)
        {
            DomainEventMap = new Dictionary<string, Type>();
            DomainEventHandlers = new Dictionary<string, Type>();
            _serviceProvider = serviceProvider;
        }

        public void Subscribe<T, TH>() where T : DomainEvent where TH : IDomainEventHandler<T>
        {
            DomainEventMap.Add(typeof(T).Name, typeof(T));
            DomainEventHandlers.Add(typeof(T).Name, typeof(TH));
        }

        public async Task InvokeHandler(string eventName, string message)
        {
            if (DomainEventMap.ContainsKey(eventName) == false)
            {
                Console.WriteLine($"--> No subscription for event: {eventName}");

                return;
            }

            var type = DomainEventMap[eventName];

            dynamic domainEvent = JsonSerializer.Deserialize(message, type);

            dynamic handler = _serviceProvider.GetService(DomainEventHandlers[eventName]);

            await handler.Handle(domainEvent);
        }
    }
}