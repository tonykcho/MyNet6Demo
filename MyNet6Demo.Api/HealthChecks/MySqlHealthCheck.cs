using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyNet6Demo.Infrastructure.DbContexts;

namespace MyNet6Demo.Api.HealthChecks
{
    public class MySqlHealthCheck : IHealthCheck
    {
        private readonly AppDbContext _context;

        public MySqlHealthCheck(AppDbContext context)
        {
            _context = context;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (await _context.Database.CanConnectAsync() == false)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, description: "Unable to connect MySql Server");
            }

            return HealthCheckResult.Healthy();
        }
    }
}