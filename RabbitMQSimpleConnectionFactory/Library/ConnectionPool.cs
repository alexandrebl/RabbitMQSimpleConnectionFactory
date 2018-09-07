﻿using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;
using System.Collections.Generic;

namespace RabbitMQSimpleConnectionFactory.Library {
    public class ConnectionPool {

        private readonly IList<IModel> _pool;
        private int _index;
        private readonly int _size;
        private static readonly object Sync = new object();

        public ConnectionPool(int size, ConnectionSetting connectionSetting) {
            _size = size;
            _pool = new List<IModel>();
            Init(size, connectionSetting, ref _pool);
        }

        public IModel GetChannel() {
            lock (Sync) {
                _index = (_index < _size) ? _index++ : _index = 0;

                return _pool[_index];
            }
        }

        private static void Init(int size, ConnectionSetting connectionSetting, ref IList<IModel> pool) {
            for (var position = 0; position < size; position++) {
                pool.Add(ChannelFactory.Create(connectionSetting));
            }
        }
    }
}