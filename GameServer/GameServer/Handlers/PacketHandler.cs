using GameServer.Hubs;
using GameServer.Models;
using GameServer.Models.Packets;
using GameServer.Stores;
using GameServer.Util;
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
            _packetFunctions.Add("REGISTER", HandleRegisterPacket);
            _packetFunctions.Add("START", HandleStartPacket);
			_packetFunctions.Add("ROLL_DICE", HandleRollDicePacket);
            _connectionMapping = connectionMapping;
        }

        public async Task HandleRegisterPacket(Packet packet, HubCallerContext context)
        {
            RegisterPacket parsed = (RegisterPacket) packet;

            string playerName = parsed.PlayerName ?? GamertagGenerator.GenerateRandomGamertag();
            string connectionId = context.ConnectionId;

            PlayerStore.Add(playerName, connectionId);
        }

        public async Task HandlePacket(Packet packet, HubCallerContext context)
        {
            // checken obs das überhaupt gibt 

            await _packetFunctions[packet.Type](packet, context);
        }

        public async Task HandleStartPacket(Packet packet, HubCallerContext context)
        {
            StartGamePacket parsed = (StartGamePacket) packet;

            Game game = GetGame(context);

            // Determine Player Order
            game.Setup();
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
	
		private async Task HandleRollDicePacket(Packet packet, HubCallerContext context)
		{
			RollDicePacket parsedpacket = (RollDicePacket)packet;

			Game game = GetGame(context);

			if (game.CurrentMover == null || game.CurrentMover.Name != parsedpacket.PlayerName)
				return;

			int rolled = game.RollDice();

			RolledPacket rolledPacket = new RolledPacket();
			rolledPacket.PlayerName = parsedpacket.PlayerName;
			rolledPacket.RolledNumber = rolled;

			string packetJson = JsonSerializer.Serialize(rolledPacket);
			await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson); 

			// game.continue after roll

			// move player to new field -> and check if passed Go field
			// check on ownership of field
			// if not owned sent packet if they want to buy
			// if owner exists than check field for current rent -> state game -> houses -> rent? -> pay Player2 so und so viel money 

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
