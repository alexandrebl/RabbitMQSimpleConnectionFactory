namespace RabbitMQSimpleConnectionFactory.Entity
{
    /// <summary>
    /// Classe de configuração de conexão do RabbitMQ
    /// </summary>
    public sealed class ConnectionSetting
    {
        /// <summary>
        /// Descrição do servidor
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Nome do VirtualHost
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// Nome do usuário do RabbitMQ
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Senha do RabbitMQ
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Porta de conexão
        /// </summary>
        public int Port { get; set; }

        public ConnectionSetting()
        {
            HostName = "localhost";
            VirtualHost = "/";
            UserName = "guest";
            Password = "guest";
            Port = 5672;
        }
    }
}
