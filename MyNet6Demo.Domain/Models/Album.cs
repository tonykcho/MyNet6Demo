using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Domain.Models
{
    public class Album : BaseEntity, IAggregateRoot
    {
        public string AlbumName { get; set; }

        public string Circle { get; set; }

        public DateTime ReleaseDate { get; set; }

        public ICollection<Song> Songs { get; set; }

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }
    }
}