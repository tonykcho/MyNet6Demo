using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Domain.Abstracts
{
    public abstract class DomainEventHandler<T> : IDomainEventHandler<T> where T : DomainEvent
    {
        public abstract Task Handle(T domainEvent);
    }
}