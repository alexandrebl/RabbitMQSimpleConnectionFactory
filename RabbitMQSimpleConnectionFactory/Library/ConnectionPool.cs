using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;
using System.Collections.Concurrent;

namespace RabbitMQSimpleConnectionFactory.Library {
    public class ConnectionPool {
        private static ConcurrentDictionary<int, IModel> _pool;
        private static int _index;
        private static int _size;
        private static readonly object Sync = new object();

        public ConnectionPool(int size, ConnectionSetting connectionSetting) {
            _size = size;

            Init(size, connectionSetting);
        }

        public IModel GetChannel() {
            lock (Sync) {
                _index = (_index < _size) ? _index++ : _index = 0;
            }

            _pool.TryGetValue(_index, out var model);
            return model;
        }

        private void Close() {
            for (var position = 0; position < _size; position++) {
                if (_pool[_index].IsOpen) {
                    _pool[_index].Close();
                }
            }
        }

        private void Init(int size, ConnectionSetting connectionSetting) {
            lock (Sync) {
                if (_pool != null) return;

                _pool = new ConcurrentDictionary<int, IModel>();

                for (var position = 0; position < size; position++) {
                    _pool.TryAdd(position, ChannelFactory.Create(connectionSetting));
                }
            }
        }
    }
}