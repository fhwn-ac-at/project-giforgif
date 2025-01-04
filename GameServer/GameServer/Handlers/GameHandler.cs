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
			game.Callback = GameEventOccured;


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
        public async Task HandlePaymentDecision(Packet packet, HubCallerContext context)
        {
            PaymentDecisionPacket parsedPacket = (PaymentDecisionPacket)packet;

			Game game = GetGame(context);

			var currentMover = game.CurrentMover;

			if (currentMover == null || currentMover.Name != parsedPacket.PlayerName)
				return;

			if (parsedPacket.WantsToBuy)
			{
				if (currentMover.BuyCurrentField())
				{
					BuySuccessfulPacket buySuccessfulPacket = new BuySuccessfulPacket();
					buySuccessfulPacket.PlayerName = currentMover.Name;
					buySuccessfulPacket.PropertyName = currentMover.CurrentPosition.Name;

					string packetJson = JsonSerializer.Serialize(buySuccessfulPacket);
					await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson);
				}
				else
				{
					BuyFailedPacket buyFailedPacket = new BuyFailedPacket();
					buyFailedPacket.PlayerName = currentMover.Name;
					buyFailedPacket.PropertyName = currentMover.CurrentPosition.Name;

					string packetJson = JsonSerializer.Serialize(buyFailedPacket);
					await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson);
				}
				return;
			}

			// Player does not want to buy, auction
			StartAuctionPacket auctionPacket = new StartAuctionPacket();
			auctionPacket.PropertyName = currentMover.CurrentPosition.Name;

			string auctionPacketJson = JsonSerializer.Serialize(auctionPacket);
			await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", auctionPacketJson);


        }
        public async Task HandleAuctionBid(Packet packet, HubCallerContext context)
        {
            AuctionBidPacket parsedPacket = (AuctionBidPacket)packet;


        }


   //     private async Task HandleAuctionResult(Packet packet, HubCallerContext context)
   //     {
   //         AuctionResultPacket parsedPacket = (AuctionResultPacket)packet;

			//Game game = GetGame(context);

			//if (game.CurrentMover.CurrentPosition != typeof(PropertyField))
   //             return;

			//game.Players.First(p => p.Name == parsedPacket.WinnerName).BuyField((PropertyField)game.CurrentMover.CurrentPosition, parsedPacket.Price);
   //     }

		private async void GameEventOccured(Packet data)
		{
			// TODO: HubCallerContext hier irgndwie rein bringen 

			//string packetJson = JsonSerializer.Serialize(data);
			//await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson); 
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
