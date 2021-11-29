using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Domain.Interfaces
{
    public interface IArtistRepository : IBaseRepository<Artist>
    {
        Task<Artist> GetArtistByNameAsync(string name, CancellationToken cancellationToken);
    }
}