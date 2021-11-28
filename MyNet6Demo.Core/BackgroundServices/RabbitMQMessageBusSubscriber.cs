using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyNet6Demo.Core.Interfaces;
using MyNet6Demo.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MyNet6Demo.Core.BackgroundServices
{
    public class RabbitMQMessageBusSubscriber : BackgroundService
    {
        private readonly IDomainEventProcessor _domainEventProcessor;
        private readonly ILogger<RabbitMQMessageBusSubscriber> _logger;
        private readonly IRabbitMQConnectionManager _rabbitMQConnectionManager;

        private IModel _channel;

        public RabbitMQMessageBusSubscriber(IDomainEventProcessor domainEventProcessor, ILogger<RabbitMQMessageBusSubscriber> logger, IRabbitMQConnectionManager rabbitMQConnectionManager)
        {
            _rabbitMQConnectionManager = rabbitMQConnectionManager;

            _domainEventProcessor = domainEventProcessor;
            
            _logger = logger;

            var connection = _rabbitMQConnectionManager.GetConnection();

            _channel = connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "direct", type: ExchangeType.Direct);

            _logger.LogInformation("--> Listening on the message bus");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            foreach (var key in _domainEventProcessor.DomainEventMap.Keys)
            {
                _channel.QueueDeclare(queue: key);

                _channel.QueueBind(queue: key, "direct", routingKey: key);

                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += async (o, args) =>
                {
                    _logger.LogInformation("--> Event Received");

                    var body = args.Body;

                    var message = Encoding.UTF8.GetString(body.ToArray());

                    await _domainEventProcessor.InvokeHandler(key, message);
                };

                _channel.BasicConsume(queue: key, autoAck: true, consumer: consumer);
            }

            return Task.CompletedTask;
        }
    }
}