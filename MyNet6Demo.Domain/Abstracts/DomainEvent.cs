public abstract class DomainEvent
{
    protected DomainEvent()
    {

    }

    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}