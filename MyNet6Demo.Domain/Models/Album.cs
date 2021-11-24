using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Domain.Models
{
    public class Album : BaseEntity, IAggregateRoot, IHasDomainEvent
    {
        public string AlbumName { get; set; }

        public string Circle { get; set; }

        public DateTime ReleaseDate { get; set; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }
    }
}