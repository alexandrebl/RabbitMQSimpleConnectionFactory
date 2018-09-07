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
            var connectionSetting = new ConnectionSetting();

            var connectionSettingDefault = new ConnectionSetting
            {
                HostName = "localhost",
                VirtualHost = "/",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };

            var channelFactory = new ChannelFactory();

            channelFactory.Create(connectionSetting);

            connectionSetting.Should().BeEquivalentTo(connectionSettingDefault);
        }
    }
}
