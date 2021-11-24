using MyNet6Demo.Domain.DomainEvents;

namespace MyNet6Demo.Domain.Interfaces
{
    public interface IHasDomainEvent
    {
        List<DomainEvent> DomainEvents { get; set; }
    }
}