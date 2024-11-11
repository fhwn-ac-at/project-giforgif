using GameServer.Hubs;
using GameServer.Models;
using GameServer.Models.Packets;
using GameServer.Stores;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace GameServer.Handlers
{
    public class PacketHandler
    {
        private readonly Dictionary<string, Func<Packet, HubCallerContext, Task>> _packetFunctions = new();
        private readonly IHubContext<Lobby> _lobbyContext;
        private readonly ConnectionMapping _connectionMapping;

        public PacketHandler(IHubContext<Lobby> lobbyContext, ConnectionMapping connectionMapping)
        {
            _lobbyContext = lobbyContext;

            // Hier ein neues packet registrieren
            _packetFunctions.Add("SAMPLE", HandleSamplePacket);
            _connectionMapping = connectionMapping;
        }

        public async Task HandlePacket(Packet packet, HubCallerContext context)
        {
            // checken obs das überhaupt gibt 

            await _packetFunctions[packet.Type](packet, context);
        }

        private async Task HandleSamplePacket(Packet packet, HubCallerContext context)
        {
            // schauen wer das packet egschickt hat und diesen player getten,
            // Das ist erlaubt weil kein bock auf visitor pattern
            SamplePacket parsed = (SamplePacket) packet;

            Game game = GetGame(context);
            // gane. ... einfach drauf reagieren

            string packetJson = JsonSerializer.Serialize(packet);
            await _lobbyContext.Clients.Group("SAMPLE").SendAsync("ReceivePacket", packetJson);
        }

        private string GetRoomName(HubCallerContext context)
        {
            _connectionMapping.TryGetRoomName(context.ConnectionId, out string roomName);
            return roomName;
        }

        private Game GetGame(HubCallerContext context)
        {
            // null checks
            string roomName = GetRoomName(context);
            return RoomStore.GetGame(roomName);
        }
    }
}
