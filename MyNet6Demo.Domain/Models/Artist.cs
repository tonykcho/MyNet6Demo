using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Domain.Models
{
    public class Artist : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }

        public ICollection<SongArtist> SongArtists { get; set; }
    }
}