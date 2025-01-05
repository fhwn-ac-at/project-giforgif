using GameServer.GameLogic;
using GameServer.Models;
using System.Collections.Concurrent;

namespace GameServer.Stores
{
    public static class PlayerStore
    {
        private static readonly ConcurrentDictionary<string, Player> _players = new();

        public static bool Add(string playerName, string connectionId)
        {
            return _players.TryAdd(connectionId, new Player(playerName, connectionId));
        }

        public static bool Exists(string connectionId) => _players.ContainsKey(connectionId);

        public static IEnumerable<string> GetAllPlayerConnections() => _players.Keys;

        public static Player GetPlayer(string connectionId)
        {
            Player? player;
            if (!_players.TryGetValue(connectionId, out player))
            {
                throw new InvalidOperationException();
            }

            return player;
        }

        public static bool Remove(string connectionId)
        {
            return _players.TryRemove(connectionId, out _);
        }
    }
}
