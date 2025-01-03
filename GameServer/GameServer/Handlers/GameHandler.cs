using GameServer.Hubs;
using GameServer.Models.Packets;
using GameServer.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using GameServer.Stores;

namespace GameServer.Handlers
{
	public class GameHandler
	{
		private readonly IHubContext<Lobby> _lobbyContext;
		private readonly ConnectionMapping _connectionMapping;

		public GameHandler(IHubContext<Lobby> lobbyContext, ConnectionMapping connectionMapping)
		{
			_lobbyContext = lobbyContext;
			_connectionMapping = connectionMapping;
		}

		public async Task HandleRollDicePacket(Packet packet, HubCallerContext context)
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

			game.MovePlayer(game.CurrentMover, rolled);
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
