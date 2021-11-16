using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyNet6Demo.Core.BackgroundServices
{
    public class SomeBackgroundService : BackgroundService
    {
        private readonly ILogger<SomeBackgroundService> _logger;

        public SomeBackgroundService(ILogger<SomeBackgroundService> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("--> Starting Background Service!");

            while (stoppingToken.IsCancellationRequested == false)
            {
                _logger.LogInformation("--> Execute Background Task");

                await Task.Delay(10000, stoppingToken);
            }

            _logger.LogInformation("--> Stopping Background Service!");
        }
    }
}