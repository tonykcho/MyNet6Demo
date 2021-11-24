using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Domain.Models
{
    public class Song : BaseEntity, IAggregateRoot, IHasDomainEvent
    {
        public int AlbumId { get; set; }

        public Album Album { get; set; }

        public string Name { get; set; }

        public int Duration { get; set; }

        public ICollection<SongArtist> SongArtists { get; set; }
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}