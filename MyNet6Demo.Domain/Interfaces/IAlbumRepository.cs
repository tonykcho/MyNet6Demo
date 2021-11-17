using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Domain.Interfaces
{
    public interface IAlbumRepository : IBaseRepository<Album>
    {
        Task<Album> GetByAlbumNameAsync(string albumName, CancellationToken cancellationToken);

        Task LoadSongsAsync(Album album, CancellationToken cancellationToken);
    }
}