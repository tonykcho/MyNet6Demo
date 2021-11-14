using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;
using MyNet6Demo.Infrastructure.DbContexts;

namespace MyNet6Demo.Infrastructure.Repositories
{
    public class AlbumRepository : BaseRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(AppDbContext context) : base(context)
        {
        }
    }
}