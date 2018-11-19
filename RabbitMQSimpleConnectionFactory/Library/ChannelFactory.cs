using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;

namespace RabbitMQSimpleConnectionFactory.Library
{
    /// <summary>
    /// Responsável por criar conexões com RabbitMQ
    /// </summary>
    public class ChannelFactory
    {

        /// <summary>
        /// Objeto
        /// </summary>
        private readonly object SyncObj = new object();

        /// <summary>
        /// Interface de conexão com RabbitMQ
        /// </summary>
        private IConnection _connection;

        private ConnectionSetting _connectionSetting;

        public ChannelFactory(ConnectionSetting connectionConfig)
        {
            this._connectionSetting = connectionConfig;
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
        public IModel Create(bool automaticRecoveryEnabled = true,
            ushort requestedHeartbeat = 0, uint requestedFrameMax = 0, ushort requestedChannelMax = 0, bool useBackgroundThreadsForIo = true)
        {
            var factory = new ConnectionFactory
            {
                HostName = this._connectionSetting.HostName,
                VirtualHost = this._connectionSetting.VirtualHost,
                UserName = this._connectionSetting.UserName,
                Password = this._connectionSetting.Password,
                Port = this._connectionSetting.Port,
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

        public IConnection CreateConnection(bool automaticRecoveryEnabled = true,
            ushort requestedHeartbeat = 0, uint requestedFrameMax = 0, ushort requestedChannelMax = 0, bool useBackgroundThreadsForIo = true)
        {

            var factory = new ConnectionFactory
            {
                HostName = this._connectionSetting.HostName,
                VirtualHost = this._connectionSetting.VirtualHost,
                UserName = this._connectionSetting.UserName,
                Password = this._connectionSetting.Password,
                Port = this._connectionSetting.Port,
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
            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }
    }
}
