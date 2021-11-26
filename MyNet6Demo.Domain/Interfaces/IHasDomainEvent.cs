using System.Text.Json.Serialization;

namespace MyNet6Demo.Domain.Interfaces
{
    public interface IHasDomainEvent
    {
        [JsonIgnore]
        List<DomainEvent> DomainEvents { get; set; }
    }
}