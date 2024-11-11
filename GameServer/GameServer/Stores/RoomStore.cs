using GameServer.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameServer.Stores
{
    public static class RoomStore
    {
        private static readonly ConcurrentDictionary<string, Game> _rooms = new();

        public static bool Add(string roomName)
        {
            return _rooms.TryAdd(roomName, new Game());
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
