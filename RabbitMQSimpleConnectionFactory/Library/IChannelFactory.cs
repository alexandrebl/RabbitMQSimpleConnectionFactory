using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;

namespace RabbitMQSimpleConnectionFactory.Library
{
    /// <summary>
    /// Responsável por criar conexões com RabbitMQ
    /// </summary>
    public interface IChannelFactory
    {
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
        IModel Create(ConnectionSetting connectionConfig, bool automaticRecoveryEnabled = true,
            ushort requestedHeartbeat = 15, uint requestedFrameMax = 0, ushort requestedChannelMax = 0, bool useBackgroundThreadsForIo = true);

        /// <summary>
        /// Método responsável por fechar a conexão com RabbitMQ
        /// </summary>
        void CloseConnection();
    }
}
