namespace MyNet6Demo.Domain.Interfaces
{
    public interface IDomainEventHandler<T> where T : DomainEvent
    {
        Task Handle(T domainEvent);
    }
}