using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Domain.DomainEvents
{
    public class AlbumCreatedEvent : DomainEvent
    {
        public Album Album { get; set; }
        public AlbumCreatedEvent(Album album)
        {
            Album = album;
        }
    }
}