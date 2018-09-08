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
        private readonly ConnectionSetting _connectionSetting = new ConnectionSetting();

        [Fact]
        public void GivenTryBuildAPoolOfConnections_WhenTheNumberOfConnectionsIsLessThan1_ThenThrowArgumentOutOfRangeException()
        {
            const int poolSize = 0;

            _mockChannelFactory.Setup(s => s.Create(It.IsAny<ConnectionSetting>(),
                    It.IsAny<bool>(),
                    It.IsAny<ushort>(),
                    It.IsAny<uint>(),
                    It.IsAny<ushort>(),
                    It.IsAny<bool>()))
                .Returns(_mockModel.Object);
           
            Assert.Throws<ArgumentOutOfRangeException>(() => new ConnectionPool(poolSize, _connectionSetting, _mockChannelFactory.Object));
        }
        
        [Fact]
        public void GivenGetChannel_WhenConnectionPoolIsValid_ThenReturnAModelOfConnection()
        {
            const int poolSize = 10;

            _mockChannelFactory.Setup(s => s.Create(It.IsAny<ConnectionSetting>(),
                    It.IsAny<bool>(),
                    It.IsAny<ushort>(),
                    It.IsAny<uint>(),
                    It.IsAny<ushort>(),
                    It.IsAny<bool>()))
                .Returns(_mockModel.Object);

            var connectionPool = new ConnectionPool(poolSize, _connectionSetting, _mockChannelFactory.Object);
            var channel = connectionPool.GetChannel();

            Assert.IsAssignableFrom<IModel>(channel);
        }
    }
}
