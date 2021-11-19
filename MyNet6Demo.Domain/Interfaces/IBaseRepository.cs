using MyNet6Demo.Domain.Abstracts;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MyNet6Demo.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<T> GetByGuidAsync(Guid guid, CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken);

        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);

        IQueryable<T> GetQuery();

        Task AddAsync(T entity, CancellationToken cancellationToken);

        void Update(T entity);

        void Remove(T entity);

        EntityEntry<T> Entry(T entity);
    }
}