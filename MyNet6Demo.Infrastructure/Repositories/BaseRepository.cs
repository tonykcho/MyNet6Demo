using Microsoft.EntityFrameworkCore;
using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Infrastructure.DbContexts;

namespace MyNet6Demo.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, IAggregateRoot
    {
        private readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _context.Set<T>().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _context.Set<T>().ToListAsync();
        }

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>();
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}