using System.Text.Json.Serialization;
using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Domain.Models
{
    public class Artist : BaseEntity, IAggregateRoot, IHasDomainEvent
    {
        public string Name { get; set; }

        public ICollection<SongArtist> SongArtists { get; set; }

        [JsonIgnore]
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}