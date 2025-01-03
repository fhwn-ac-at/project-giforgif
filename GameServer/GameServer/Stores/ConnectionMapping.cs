using GameServer.Models;
using System.Collections.Concurrent;

namespace GameServer.Stores
{
    public class ConnectionMapping
    {
        private readonly ConcurrentDictionary<string, string> _connections = new();

        public void Add(string connectionId, string roomName)
        {
            _connections[connectionId] = roomName;
        }

        public bool TryGetRoomName(string connectionId, out string roomName)
        {
            return _connections.TryGetValue(connectionId, out roomName);
        }

        public void Remove(string connectionId)
        {
            _connections.TryRemove(connectionId, out _);
        }
    }
}
