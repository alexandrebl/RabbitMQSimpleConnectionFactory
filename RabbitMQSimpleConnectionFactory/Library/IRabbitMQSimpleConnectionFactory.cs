using RabbitMQ.Client;

namespace RabbitMQSimpleConnectionFactory.Library
{
    public interface IRabbitMQSimpleConnectionFactory : IConnectionFactory
    {
        string HostName { get; set; }
        int Port { get; set; }
        bool AutomaticRecoveryEnabled { get; set; }
    }
}
