using Microsoft.EntityFrameworkCore;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;
using MyNet6Demo.Infrastructure.DbContexts;

namespace MyNet6Demo.Infrastructure.Repositories
{
    public class ArtistRepository : BaseRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Artist> GetArtistByNameAsync(string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _context.Artists.SingleOrDefaultAsync(artist => artist.Name == name, cancellationToken);
        }
    }
}