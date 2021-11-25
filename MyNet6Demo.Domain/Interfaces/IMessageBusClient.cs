namespace MyNet6Demo.Domain.Interfaces
{
    public interface IMessageBusClient
    {
        Task PublishDomainEventAsync<T>(T domainEvent, CancellationToken cancellationToken) where T : DomainEvent;
    }
}