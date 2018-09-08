using Moq;
using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;
using RabbitMQSimpleConnectionFactory.Library;
using System;
using Xunit;


namespace RabbitMQSimpleConnectionFactory.UnitTests
{
    public class ConnectionPoolTests
    {
        private readonly Mock<IChannelFactory> _mockChannelFactory = new Mock<IChannelFactory>();
        private readonly Mock<IModel> _mockModel = new Mock<IModel>();

        [Fact]
        public void GivenTryBuildAPooOfConnections_WhenTheNumberOfConnectionsIsLessThan1_ThenThrowArgumentOutOfRangeException()
        {
            const int poolSize = 0;
            var connectionSetting = new ConnectionSetting();

            _mockChannelFactory.Setup(s => s.Create(It.IsAny<ConnectionSetting>(),
                    It.IsAny<bool>(),
                    It.IsAny<ushort>(),
                    It.IsAny<uint>(),
                    It.IsAny<ushort>(),
                    It.IsAny<bool>()))
                .Returns(_mockModel.Object);
           
            Assert.Throws<ArgumentOutOfRangeException>(() => new ConnectionPool(poolSize, connectionSetting, _mockChannelFactory.Object));
        }
    }
}
