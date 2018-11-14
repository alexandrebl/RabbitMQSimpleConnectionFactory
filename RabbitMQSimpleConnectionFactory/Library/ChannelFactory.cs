using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;

namespace RabbitMQSimpleConnectionFactory.Library
{
    /// <summary>
    /// Responsável por criar conexões com RabbitMQ
    /// </summary>
    public static class ChannelFactory
    {

        /// <summary>
        /// Objeto
        /// </summary>
        private static readonly object SyncObj = new object();

        /// <summary>
        /// Interface de conexão com RabbitMQ
        /// </summary>
        private static IConnection _connection;

        /// <summary>
        /// Método cria uma conexão com RabbitMQ
        /// </summary>
        /// <param name="connectionConfig">Configurações de conexão</param>
        /// <param name="automaticRecoveryEnabled"></param>
        /// <param name="requestedHeartbeat"></param>
        /// <param name="requestedFrameMax"></param>
        /// <param name="requestedChannelMax"></param>
        /// <param name="useBackgroundThreadsForIo"></param>
        /// <returns></returns>
        public static IModel Create(ConnectionSetting connectionConfig, bool automaticRecoveryEnabled = true,
            ushort requestedHeartbeat = 0, uint requestedFrameMax = 0, ushort requestedChannelMax = 0, bool useBackgroundThreadsForIo = true)
        {
            var factory = new ConnectionFactory
            {
                HostName = connectionConfig.HostName,
                VirtualHost = connectionConfig.VirtualHost,
                UserName = connectionConfig.UserName,
                Password = connectionConfig.Password,
                Port = connectionConfig.Port,
                AutomaticRecoveryEnabled = automaticRecoveryEnabled,
                RequestedHeartbeat = requestedHeartbeat,
                RequestedFrameMax = requestedFrameMax,
                RequestedChannelMax = requestedChannelMax,
                UseBackgroundThreadsForIO = useBackgroundThreadsForIo,
                Protocol = Protocols.AMQP_0_9_1
            };

            if (_connection == null || !_connection.IsOpen)
            {
                lock (SyncObj)
                {
                    if (_connection == null || !_connection.IsOpen)
                    {
                        _connection = factory.CreateConnection();
                    }
                }
            }

            var channel = _connection.CreateModel();

            return channel;
        }

        public static IModel Create(IConnection connection, bool automaticRecoveryEnabled = true,
            ushort requestedHeartbeat = 0, uint requestedFrameMax = 0, ushort requestedChannelMax = 0, bool useBackgroundThreadsForIo = true)
        {

            var channel = connection.CreateModel();

            return channel;
        }

        public static IConnection CreateConnection(ConnectionSetting connectionConfig, bool automaticRecoveryEnabled = true,
            ushort requestedHeartbeat = 0, uint requestedFrameMax = 0, ushort requestedChannelMax = 0, bool useBackgroundThreadsForIo = true)
        {

            var factory = new ConnectionFactory
            {
                HostName = connectionConfig.HostName,
                VirtualHost = connectionConfig.VirtualHost,
                UserName = connectionConfig.UserName,
                Password = connectionConfig.Password,
                Port = connectionConfig.Port,
                AutomaticRecoveryEnabled = automaticRecoveryEnabled,
                RequestedHeartbeat = requestedHeartbeat,
                RequestedFrameMax = requestedFrameMax,
                RequestedChannelMax = requestedChannelMax,
                UseBackgroundThreadsForIO = useBackgroundThreadsForIo,
                Protocol = Protocols.AMQP_0_9_1
            };

            var connection = factory.CreateConnection();

            return connection;
        }

        /// <summary>
        /// Método responsável por fechar a conexão com RabbitMQ
        /// </summary>
        public static void CloseConnection()
        {
            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }
    }
}
