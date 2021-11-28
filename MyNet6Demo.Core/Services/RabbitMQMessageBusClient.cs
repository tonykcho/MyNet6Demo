using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyNet6Demo.Core.Interfaces;
using MyNet6Demo.Domain.Interfaces;
using RabbitMQ.Client;

namespace MyNet6Demo.Core.Services
{
    public class RabbitMQMessageBusClient : IMessageBusClient
    {
        private readonly ILogger<RabbitMQMessageBusClient> _logger;
        private readonly IRabbitMQConnectionManager _rabbitMQConnectionManager;
        private readonly IModel _channel;

        public RabbitMQMessageBusClient(ILogger<RabbitMQMessageBusClient> logger, IRabbitMQConnectionManager rabbitMQConnectionManager)
        {
            _logger = logger;

            _rabbitMQConnectionManager = rabbitMQConnectionManager;

            _channel = _rabbitMQConnectionManager.GetConnection().CreateModel();

            _channel.ExchangeDeclare(exchange: "direct", type: ExchangeType.Direct);
        }

        public Task PublishDomainEventAsync<T>(T domainEvent, CancellationToken cancellationToken) where T : DomainEvent
        {
            cancellationToken.ThrowIfCancellationRequested();

            string message = JsonSerializer.Serialize(domainEvent, domainEvent.GetType());

            if (_channel.IsOpen)
            {
                _logger.LogInformation("--> RabbitMQ Connection Open, sending message...");

                var body = Encoding.UTF8.GetBytes(message);

                var queueName = domainEvent.GetType().Name;

                _logger.LogInformation($"--> Queue Name is {queueName}");

                _channel.BasicPublish(exchange: "direct", routingKey: queueName, body: body);
            }
            else
            {
                _logger.LogInformation("--> RabbitMQ Connection Closed, not sending...");
            }

            return Task.CompletedTask;
        }

        // public void Dispose()
        // {
        //     _logger.LogInformation("--> RabbitMQ MessageBus Disposed");

        //     if (_channel.IsOpen)
        //     {
        //         _channel.Close();

        //         _connection.Close();
        //     }
        // }
    }
}