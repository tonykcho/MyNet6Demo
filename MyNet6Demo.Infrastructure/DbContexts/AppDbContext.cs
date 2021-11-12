using System.Transactions;
using Microsoft.EntityFrameworkCore;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace MyNet6Demo.Infrastructure.DbContexts
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        public DbSet<Album> Albums { get; set; }

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            Console.WriteLine(_configuration.GetConnectionString("App"));

            builder.UseMySql(_configuration.GetConnectionString("App"), new MySqlServerVersion(new Version(8, 0, 27)));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            using (var transaction = await Database.BeginTransactionAsync())
            {
                await action();

                await transaction.CommitAsync();
            }

            // using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            // {
            //     await action();

            //     scope.Complete();
            // }
        }

        public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            using (var transaction = await Database.BeginTransactionAsync())
            {
                TResult result = await action();

                await transaction.CommitAsync();

                return result;
            }
            // using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            // {
            //     TResult result = await action();

            //     scope.Complete();

            //     return result;
            // }
        }
    }
}