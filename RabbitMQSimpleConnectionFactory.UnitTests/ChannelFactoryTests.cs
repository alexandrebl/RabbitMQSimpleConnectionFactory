using FluentAssertions;
using Moq;
using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;
using RabbitMQSimpleConnectionFactory.Library;
using Xunit;

namespace RabbitMQSimpleConnectionFactory.UnitTests
{
    public class ChannelFactoryTests
    {
        private readonly Mock<IConnectionFactory> _mockConnectionFactory;

        private readonly ChannelFactory _channelFactory;

        public ChannelFactoryTests()
        {
            _mockConnectionFactory = new Mock<IConnectionFactory>();
            _channelFactory = new ChannelFactory(_mockConnectionFactory.Object);
        }

        [Fact]
        public void GivenGetChannel_WhenNotSetConnectionSetting_ThenConnectionSettingUseDefault()
        {
            var connectionSettingActual = new ConnectionSetting();

            var connectionSettingDefaultExpected = new ConnectionSetting
            {
                HostName = "localhost",
                VirtualHost = "/",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
            
            _channelFactory.Create(connectionSettingActual);

            connectionSettingActual.Should().BeEquivalentTo(connectionSettingDefaultExpected);
        }

        [Fact]
        public void GivenAConnectionOpen_WhenCallMethodToCloseConnection_ThenTheConnectionReturns()
        {
            var mockConnection = new Mock<IConnection>();
            var mockModel = new Mock<IModel>();

            _mockConnectionFactory.Setup(s => s.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(s => s.CreateModel()).Returns(mockModel.Object);

            _channelFactory.CloseConnection();

            Assert.Null(_channelFactory.Connection);
        }
    }
}
