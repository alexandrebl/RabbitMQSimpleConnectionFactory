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

        [Fact]
        public void GivenCreateConnectionSetting_WhenNotSetConnectionSetting_ThenConnectionSettingUseDefault()
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

            connectionSettingActual.Should().BeEquivalentTo(connectionSettingDefaultExpected);
        }


        [Fact]
        public void GivenGetChannel_WhenSetConnectionSetting_ThenConnectionSettingUseTheParameterValue()
        {
            var connectionSettingActual = new ConnectionSetting
            {
                HostName = "HostName",
                VirtualHost = "VirtualHost",
                UserName = "UserName",
                Password = "Password",
                Port = 1234
            };

            var connectionSettingDefaultExpected = new ConnectionSetting
            {
                HostName = "HostName",
                VirtualHost = "VirtualHost",
                UserName = "UserName",
                Password = "Password",
                Port = 1234
            };

            connectionSettingActual.Should().BeEquivalentTo(connectionSettingDefaultExpected);
        }

        [Fact]
        public void GivenAConnectionOpen_WhenCallMethodToCloseConnection_ThenTheConnectionReturns()
        {
            var mockConnection = new Mock<IConnection>();
            var mockRabbitMQSimpleConnectionFactory = new Mock<IRabbitMQSimpleConnectionFactory>();
            mockRabbitMQSimpleConnectionFactory.Setup(s => s.CreateConnection()).Returns(mockConnection.Object);

            var channelFactory = new ChannelFactory(mockRabbitMQSimpleConnectionFactory.Object);
            var connectionSettingActual = new ConnectionSetting();
            
            channelFactory.Create(connectionSettingActual);
            channelFactory.CloseConnection();

            Assert.Null(channelFactory.Connection);
        }

        [Fact]
        public void GivenTryCreateChannel_WhenConnectionCreationWithRabbitMQServerFails_ThenIsThrowRabbitMQClientExceptionsConnectFailureException()
        {
            var mockRabbitMQSimpleConnectionFactory = new Mock<IRabbitMQSimpleConnectionFactory>();
            var channelFactory = new ChannelFactory(mockRabbitMQSimpleConnectionFactory.Object);
            var connectionSettingActual = new ConnectionSetting();

            mockRabbitMQSimpleConnectionFactory.Setup(s => s.CreateConnection()).Returns<IConnection>(null);

            var result = Assert.Throws<RabbitMQ.Client.Exceptions.ConnectFailureException>(() => channelFactory.Create(connectionSettingActual));

            Assert.Equal(result.Message, $"Could not create connection to RabbitMQ Server.");
        }
    }
}
