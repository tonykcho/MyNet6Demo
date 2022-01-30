using System.Net.Sockets;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyNet6Demo.Api.HealthChecks
{
    public class ElasticSearchHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public ElasticSearchHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var client = new TcpClient(_configuration["ElasticSearchHost"], _configuration.GetValue<int>("ElasticSearchPort")))
                {
                    return await Task.FromResult(HealthCheckResult.Healthy());
                }
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }
}