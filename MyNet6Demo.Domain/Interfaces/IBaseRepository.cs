using MyNet6Demo.Domain.Abstracts;

namespace MyNet6Demo.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken);

        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);

        IQueryable<T> GetQuery();
    }
}