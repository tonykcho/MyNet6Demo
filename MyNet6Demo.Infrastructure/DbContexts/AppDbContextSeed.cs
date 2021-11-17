namespace MyNet6Demo.Infrastructure.DbContexts
{
    public class AppDbContextSeed
    {
        public static async Task SeedSampleDataAsync(AppDbContext context)
        {
            if (context.Albums.Any() == false)
            {
                //Seed Sample
            }

            await context.SaveChangesAsync();
        }
    }
}