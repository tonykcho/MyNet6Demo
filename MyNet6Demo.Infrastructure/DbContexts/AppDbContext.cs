using Microsoft.EntityFrameworkCore;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using MyNet6Demo.Domain.Abstracts;

namespace MyNet6Demo.Infrastructure.DbContexts
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageBusClient _messageBusClient;
        public DbSet<Album> Albums { get; set; }

        public AppDbContext(IConfiguration configuration, IMessageBusClient messageBusClient)
        {
            _configuration = configuration;
            _messageBusClient = messageBusClient;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseMySql(_configuration.GetConnectionString("App"), new MySqlServerVersion(new Version(8, 0, 27)));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var transaction = await Database.BeginTransactionAsync(cancellationToken))
            {
                await action();

                await transaction.CommitAsync(cancellationToken);
            }

            // using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            // {
            //     await action();

            //     scope.Complete();
            // }
        }

        public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var transaction = await Database.BeginTransactionAsync(cancellationToken))
            {
                TResult result = await action();

                await transaction.CommitAsync(cancellationToken);

                return result;
            }
            // using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            // {
            //     TResult result = await action();

            //     scope.Complete();

            //     return result;
            // }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
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

            var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .ToArray();

            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents(events, cancellationToken);

            return result;
        }

        private async Task DispatchEvents<T>(T[] events, CancellationToken cancellationToken) where T : DomainEvent
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            foreach(var domainEvent in events)
            {
                await _messageBusClient.PublishDomainEventAsync(domainEvent, cancellationToken);
            }
        }
    }
}