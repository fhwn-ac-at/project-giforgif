using Microsoft.AspNetCore.SignalR;
using GameServer.Stores;
using GameServer.Models.Packets;
using System.Text.Json;
using GameServer.Handlers;
using GameServer.Models;

namespace GameServer.Hubs
{
    public class Lobby : Hub
    {
        private readonly PacketHandler _packetHandler;
        private readonly ConnectionMapping _connectionMapping;

        public Lobby(PacketHandler packetHandler, ConnectionMapping connectionMapping)
        {
            _packetHandler = packetHandler;
            _connectionMapping = connectionMapping;
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ShowWhoLeft", $"{Context.ConnectionId} has left '{roomName}'.");
            _connectionMapping.Remove(Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_connectionMapping.TryGetRoomName(Context.ConnectionId, out string roomName))
            {
                await LeaveRoom(roomName);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendPacketToServer(string packetJson)
        {
            var packet = JsonSerializer.Deserialize<Packet>(packetJson, new JsonSerializerOptions
            {
                Converters = { new PacketJsonConverter() },
                PropertyNameCaseInsensitive = true
            });

            if (packet != null)
            {
                await _packetHandler.HandlePacket(packet, Context);
                return;
            }

            await Clients.Caller.SendAsync("Error", "Ungültiges Paketformat.");

        }

        public async Task SendPacketToRoom(string roomName, Packet packet)
        {
            string packetJson = JsonSerializer.Serialize(packet);
            await Clients.Group(roomName).SendAsync("ReceivePacket", packetJson);
        }
    }
}
