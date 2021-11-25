using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MyNet6Demo.Core.BackgroundServices
{
    public class RabbitMQMessageBusSubscriber : BackgroundService
    {
        private readonly IDomainEventProcessor _domainEventProcessor;

        private readonly ILogger<RabbitMQMessageBusSubscriber> _logger;

        private IConnection _connection;

        private IModel _channel;

        public RabbitMQMessageBusSubscriber(IDomainEventProcessor domainEventProcessor, ILogger<RabbitMQMessageBusSubscriber> logger)
        {
            _domainEventProcessor = domainEventProcessor;
            _logger = logger;

            InitializeRabbitMQ();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            foreach (var key in _domainEventProcessor.DomainEventMap.Keys)
            {
                _channel.QueueDeclare(queue: key);

                _channel.QueueBind(key, "trigger", routingKey: key);

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

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "Hkc64760575" };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            Console.WriteLine("--> Listening on the message bus");

            _connection.ConnectionShutdown += OnRabbitMQ_ConnectionShutdown;
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