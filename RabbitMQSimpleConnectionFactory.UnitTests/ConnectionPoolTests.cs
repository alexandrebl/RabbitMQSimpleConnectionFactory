using AutoFixture;
using FluentAssertions;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.Impl;
using RabbitMQSimpleConnectionFactory.Entity;
using RabbitMQSimpleConnectionFactory.Library;
using Xunit;


namespace RabbitMQSimpleConnectionFactory.UnitTests
{
    public class ConnectionPoolTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IChannelFactory> _mockChannelFactory = new Mock<IChannelFactory>();
        private readonly Mock<IModel> _mockModel = new Mock<IModel>();

        [Fact]
        public void GivenGetChannel_WhenNotSetConnectionSetting_ThenConnectionSettingUseDefault()
        {
            const int poolSize = 1;
            var connectionSetting = new ConnectionSetting();
            var connectionSettingDefault = new ConnectionSetting
            {
                HostName = "localhost",
                VirtualHost = "/",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };

            _mockChannelFactory.Setup(s => s.Create(It.IsAny<ConnectionSetting>(),
                    It.IsAny<bool>(),
                    It.IsAny<ushort>(),
                    It.IsAny<uint>(),
                    It.IsAny<ushort>(),
                    It.IsAny<bool>()))
                .Returns(_mockModel.Object);

            var connectionPool = new ConnectionPool(poolSize, connectionSetting, _mockChannelFactory.Object);

            connectionSetting.Should().BeEquivalentTo(connectionSettingDefault);
        }
    }
}
