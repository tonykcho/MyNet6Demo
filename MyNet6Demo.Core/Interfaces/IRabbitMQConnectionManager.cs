using RabbitMQ.Client;

namespace MyNet6Demo.Core.Interfaces
{
    public interface IRabbitMQConnectionManager
    {
        IConnection GetConnection();
        IModel GetChannel();
    }
}