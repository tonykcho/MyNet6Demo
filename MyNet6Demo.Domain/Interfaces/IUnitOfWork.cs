namespace MyNet6Demo.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task ExecuteAsync(Func<Task> action);

        Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);
    }
}