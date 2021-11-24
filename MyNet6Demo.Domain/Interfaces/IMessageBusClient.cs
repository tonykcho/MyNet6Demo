namespace MyNet6Demo.Domain.Interfaces
{
    public interface IMessageBusClient
    {
        Task PublishDomainEventAsync(DomainEvent domainEvent, CancellationToken cancellationToken);
    }
}