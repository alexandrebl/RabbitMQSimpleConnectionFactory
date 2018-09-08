using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;

namespace RabbitMQSimpleConnectionFactory.Library
{
    /// <summary>
    /// Responsável por criar conexões com RabbitMQ
    /// </summary>
    public class ChannelFactory : IChannelFactory
    {
        /// <summary>
        /// Objeto
        /// </summary>
        private static readonly object SyncObj = new object();

        /// <summary>
        /// Interface de conexão com RabbitMQ
        /// </summary>
        public IConnection Connection;

        /// <summary>
        /// Interface de construções de conexões do rabbit
        /// </summary>
        private IConnectionFactory _connectionFactory;

        public ChannelFactory(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

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
        public IModel Create(ConnectionSetting connectionConfig, bool automaticRecoveryEnabled = true,
            ushort requestedHeartbeat = 15, uint requestedFrameMax = 0, ushort requestedChannelMax = 0, bool useBackgroundThreadsForIo = true)
        {
            _connectionFactory = new ConnectionFactory
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

            if (Connection == null)
            {
                lock (SyncObj)
                {
                    if (Connection == null)
                    {
                        Connection = _connectionFactory.CreateConnection();
                    }
                }
            }

            var channel = Connection.CreateModel();

            return channel;
        }

        public static IModel Create(IConnection connection, bool automaticRecoveryEnabled = true,
            ushort requestedHeartbeat = 15, uint requestedFrameMax = 0, ushort requestedChannelMax = 0, bool useBackgroundThreadsForIo = true) {

            var channel = connection.CreateModel();

            return channel;
        }

        public static IConnection CreateConnection(ConnectionSetting connectionConfig, bool automaticRecoveryEnabled = true,
            ushort requestedHeartbeat = 15, uint requestedFrameMax = 0, ushort requestedChannelMax = 0, bool useBackgroundThreadsForIo = true) {

            var factory = new ConnectionFactory {
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
        public void CloseConnection()
        {
            Connection?.Close();
            Connection?.Dispose();
            Connection = null;
        }
    }
}
