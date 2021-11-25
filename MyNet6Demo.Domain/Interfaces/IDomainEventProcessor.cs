namespace MyNet6Demo.Domain.Interfaces
{
    public interface IDomainEventProcessor
    {
        IDictionary<string, Type> DomainEventMap { get; set; }
        void Subscribe<T, TH>() where T : DomainEvent where TH : IDomainEventHandler<T>;

        Task InvokeHandler(string eventName, string message);
    }
}