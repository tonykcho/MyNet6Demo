using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MyNet6Demo.Core.BackgroundServices
{
    public class RabbitMQMessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IDomainEventProcessor _domainEventProcessor;

        private readonly ILogger<RabbitMQMessageBusSubscriber> _logger;

        private IConnection _connection;

        private IModel _channel;

        public RabbitMQMessageBusSubscriber(IDomainEventProcessor domainEventProcessor, ILogger<RabbitMQMessageBusSubscriber> logger, IConfiguration configuration)
        {
            _domainEventProcessor = domainEventProcessor;
            _configuration = configuration;
            _logger = logger;

            InitializeRabbitMQ();
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

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "direct", type: ExchangeType.Direct);

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