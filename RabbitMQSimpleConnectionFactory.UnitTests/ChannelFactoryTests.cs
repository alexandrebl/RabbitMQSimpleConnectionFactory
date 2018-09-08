using FluentAssertions;
using RabbitMQSimpleConnectionFactory.Entity;
using RabbitMQSimpleConnectionFactory.Library;
using Xunit;

namespace RabbitMQSimpleConnectionFactory.UnitTests
{
    public class ChannelFactoryTests
    {
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

            var channelFactory = new ChannelFactory();

            channelFactory.Create(connectionSettingActual);

            connectionSettingActual.Should().BeEquivalentTo(connectionSettingDefaultExpected);
        }

        [Fact]
        public void GivenAChannelOpen_WhenCallMethodToCloseConnection_ThenTheConnectionIsClosed()
        {
            var connectionSetting = new ConnectionSetting();
            var channelFactory = new ChannelFactory();
            var channel = channelFactory.Create(connectionSetting);

            channelFactory.CloseConnection();

            Assert.True(channel.IsClosed);
        }
    }
}
