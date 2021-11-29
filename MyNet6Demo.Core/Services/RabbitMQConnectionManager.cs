using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyNet6Demo.Core.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MyNet6Demo.Core.Services
{
    public class RabbitMQConnectionManager : IRabbitMQConnectionManager
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQConnectionManager> _logger;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQConnectionManager(IConfiguration configuration, ILogger<RabbitMQConnectionManager> logger)
        {
            _configuration = configuration;

            _logger = logger;

            _factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                UserName = "guest",
                Password = "guest"
            };

            try
            {
                _connection = _factory.CreateConnection();

                _logger.LogInformation("--> Connected To RabbitMQ");

                _connection.ConnectionShutdown += OnRabbitMQ_ConnectionShutdown;

                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> Could not connect to the message bus: {ex.Message}");
            }
        }

        public IConnection GetConnection()
        {
            return _connection;
        }

        public IModel GetChannel()
        {
            return _channel;
        }

        private void OnRabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs args)
        {
            _logger.LogInformation("--> RabbitMQ Connection Shutdown");
        }
    }
}