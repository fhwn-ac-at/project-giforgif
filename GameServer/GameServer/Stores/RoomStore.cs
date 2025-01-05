using GameServer.GameLogic;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameServer.Stores
{
    public static class RoomStore
    {
        private static readonly ConcurrentDictionary<string, Game> _rooms = new();

        public static Game? Add(string roomName)
        {
            Game game = new Game();

            if(!_rooms.TryAdd(roomName, game))
            {
                return null;
            }

            return game;
        }

        public static bool Exists(string roomName) => _rooms.ContainsKey(roomName);

        public static IEnumerable<string> GetAllRoomNames() => _rooms.Keys;

        public static Game GetGame(string roomName)
        {
            _rooms.TryGetValue(roomName, out var game);
            return game;
        }

        public static bool Remove(string roomName)
        {
            return _rooms.TryRemove(roomName, out _);
        }
    }
}
