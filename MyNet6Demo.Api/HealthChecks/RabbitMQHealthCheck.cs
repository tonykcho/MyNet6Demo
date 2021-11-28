using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyNet6Demo.Core.Interfaces;
using RabbitMQ.Client;

namespace MyNet6Demo.Api.HealthChecks
{
    public class RabbitMQHealthCheck : IHealthCheck
    {
        private readonly IRabbitMQConnectionManager _rabbitMQConnectionManager;

        public RabbitMQHealthCheck(IRabbitMQConnectionManager rabbitMQConnectionManager)
        {
            _rabbitMQConnectionManager = rabbitMQConnectionManager;
        }
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var connection = _rabbitMQConnectionManager.GetConnection();

            if (connection.IsOpen == false)
            {
                return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, description: "Unable to connect RabbitMQ Server"));
            }

            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}