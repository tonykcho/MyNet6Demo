using MyNet6Demo.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MyNet6Demo.Api.Extensions
{
    public static class WebApplicationExtension
    {
        public static async Task MigrateSchemaAsync(this WebApplication app)
        {
            await using (var scope = app.Services.CreateAsyncScope())
            {
                ILogger<WebApplication> logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    logger.LogInformation("Start Migrating AppDbContext Schema");

                    await context.Database.MigrateAsync();

                    await AppDbContextSeed.SeedSampleDataAsync(context);

                    logger.LogInformation("Migrate AppDbContext Success");
                }
                catch(Exception ex)
                {
                    // logger.LogError(ex.Message, "An error occurred while migrating the database used on context");
                }
            }
        }
    }
}