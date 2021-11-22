using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyNet6Demo.Domain.Abstracts;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Infrastructure.DbContexts;

namespace MyNet6Demo.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _context.Set<T>().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual async Task<T> GetByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _context.Set<T>().SingleOrDefaultAsync(x => x.Guid == guid, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>();
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var entry in _context.ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastUpdatedAt = DateTime.UtcNow;
                        break;
                }
            }

            return (await _context.SaveChangesAsync(cancellationToken)) > 0;
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public EntityEntry<T> Entry(T entity)
        {
            return _context.Entry(entity);
        }
    }
}