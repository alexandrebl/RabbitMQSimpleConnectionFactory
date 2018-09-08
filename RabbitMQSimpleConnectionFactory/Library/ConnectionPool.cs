using System;
using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;
using System.Collections.Generic;

namespace RabbitMQSimpleConnectionFactory.Library {
    public class ConnectionPool {

        private readonly IList<IModel> _pool;
        private int _index;
        private readonly int _size;
        private static readonly object Sync = new object();
        private readonly IChannelFactory _channelFactory;

        public ConnectionPool(int size, ConnectionSetting connectionSetting, IChannelFactory channelFactory) {
            _size = size;
            _pool = new List<IModel>();
            _channelFactory = channelFactory;
            Init(size, connectionSetting, ref _pool);

        }

        public IModel GetChannel() {
            lock (Sync) {
                _index = (_index < _size) ? _index++ : _index = 0;

                return _pool[_index];
            }
        }

        private void Init(int size, ConnectionSetting connectionSetting, ref IList<IModel> pool) {
            if (size < 1)
            {
                throw new ArgumentOutOfRangeException("size", size,
                    "size of the connection pool must be equal to or greater than 1");
            }

            for (var position = 0; position < size; position++) {
                pool.Add(_channelFactory.Create(connectionSetting));
            }
        }
    }
}