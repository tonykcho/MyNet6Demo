using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Interfaces;
using RabbitMQ.Client;

namespace MyNet6Demo.Core.Services
{
    public class RabbitMQMessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQMessageBusClient> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQMessageBusClient(IConfiguration configuration, ILogger<RabbitMQMessageBusClient> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,

                // HostName = _configuration["RabbitMQHost"],
                // Port = int.Parse(_configuration["RabbitMQPort"]),
                UserName = "guest",
                Password = "Hkc64760575"
            };

            try
            {
                _connection = factory.CreateConnection();

                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += OnRabbitMQ_ConnectionShutdown;

                _logger.LogInformation("--> Connected To RabbitMQ");
            }
            catch (Exception ex)
            {
                // _logger.LogInformation($"{_configuration["RabbitMQHost"]}");

                // _logger.LogInformation($"{_configuration["RabbitMQPort"]}");

                _logger.LogError($"--> Could not connect to the message bus: {ex.Message}");
            }
        }

        public async Task PublishDomainEventAsync(DomainEvent domainEvent, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            string message = JsonSerializer.Serialize(domainEvent);

            if (_connection.IsOpen)
            {
                _logger.LogInformation("--> RabbitMQ Connection Open, sending message...");

                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: "trigger", routingKey: nameof(domainEvent), body: body);

                _logger.LogInformation($"--> We have sent {message}");
            }
            else
            {
                _logger.LogInformation("--> RabbitMQ Connection Closed, not sending...");
            }
        }

        public void Dispose()
        {
            _logger.LogInformation("--> RabbitMQ MessageBus Disposed");

            if (_channel.IsOpen)
            {
                _channel.Close();

                _connection.Close();
            }
        }

        private void OnRabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs args)
        {
            _logger.LogInformation("--> RabbitMQ Connection Shutdown");
        }
    }
}