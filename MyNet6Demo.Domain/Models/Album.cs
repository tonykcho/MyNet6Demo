using System.Text.Json.Serialization;
using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Domain.Models
{
    public class Album : BaseEntity, IAggregateRoot, IHasDomainEvent
    {
        public string AlbumName { get; set; }

        public string Circle { get; set; }

        public string? Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string? ImagePath { get; set; }

        public string? ImageContentType { get; set; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();

        [JsonIgnore]
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }
    }
}