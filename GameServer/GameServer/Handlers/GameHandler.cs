using GameServer.Hubs;
using GameServer.Models.Packets;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using GameServer.Stores;
using GameServer.GameLogic;
using GameServer.Models.Fields;

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
			game.FieldEventOccurred += async (sender, packet) => await Game_FieldEventOccurredAsync(sender, context, packet);
			//game.Callback = (packet) => GameEventOccured(context, packet);

			Player player = PlayerStore.GetPlayer(context.ConnectionId);

			if (game.CurrentMover == null || game.CurrentMover != player)
				return;

			int rolled = game.RollDice();

			RolledPacket rolledPacket = new RolledPacket();
			rolledPacket.PlayerName = game.CurrentMover.Name;
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

		private async Task Game_FieldEventOccurredAsync(object? sender, HubCallerContext context, Packet e)
		{
			if (e is BuyRequestPacket buyRequestPacket)
			{
				string packet = JsonSerializer.Serialize(e);
				await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", packet);
			}

			string packetJson = JsonSerializer.Serialize(e);
			await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson);
		}

		public async Task HandlePaymentDecision(Packet packet, HubCallerContext context)
        {
            PaymentDecisionPacket parsedPacket = (PaymentDecisionPacket)packet;

			Game game = GetGame(context);
			Player player = PlayerStore.GetPlayer(context.ConnectionId);

			var currentMover = game.CurrentMover;

			if (currentMover == null || currentMover != player)
				return;

			if (parsedPacket.WantsToBuy)
			{
				PropertyField field = (PropertyField)player.Board.GetFieldById(player.CurrentPositionFieldId);

				if (currentMover.BuyField(field, field.BuyingPrice))
				{
					BoughtFieldPacket buySuccessfulPacket = new BoughtFieldPacket();
					buySuccessfulPacket.PlayerName = currentMover.Name;
					buySuccessfulPacket.FieldId = currentMover.CurrentPositionFieldId;
					
					string packetJson = JsonSerializer.Serialize(buySuccessfulPacket);
					await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson);
					return;
				}
				else
				{
					//BuyFailedPacket buyFailedPacket = new BuyFailedPacket();
					//buyFailedPacket.PlayerName = currentMover.Name;
					//buyFailedPacket.PropertyName = currentMover.CurrentPosition.Name;

					//string packetJson = JsonSerializer.Serialize(buyFailedPacket);
					//await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson);
					await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("BUY_FAILED", "You can not afford to buy this property.")));
				}
			}

			// Player does not want to buy, auction
			StartAuctionPacket auctionPacket = new StartAuctionPacket();
			auctionPacket.FieldId = currentMover.CurrentPositionFieldId;

			string auctionPacketJson = JsonSerializer.Serialize(auctionPacket);
			await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", auctionPacketJson);

		}
		public async Task HandleAuctionBid(Packet packet, HubCallerContext context)
        {
            AuctionBidPacket parsedPacket = (AuctionBidPacket)packet;

			// start biding thread und immer packets abfangen 
			// wenn nach 10 sek kein aaction bid packet kommt, wird autionresult geschickt + gewinner kauft feld logic 
        }


   //     private async Task HandleAuctionResult(Packet packet, HubCallerContext context)
   //     {
   //         AuctionResultPacket parsedPacket = (AuctionResultPacket)packet;

			//Game game = GetGame(context);

			//if (game.CurrentMover.CurrentPosition != typeof(PropertyField))
   //             return;

			//game.Players.First(p => p.Name == parsedPacket.WinnerName).BuyField((PropertyField)game.CurrentMover.CurrentPosition, parsedPacket.Price);
   //     }

		//private async void GameEventOccured(HubCallerContext context, Packet data)
		//{
		//	string packetJson = JsonSerializer.Serialize(data);
		//	await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson);
		//}

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

		public async Task HandleBuilding(Packet packet, HubCallerContext context)
		{
			BuildHousePacket parsedPacket = (BuildHousePacket)packet;
			string connectionId = context.ConnectionId;

			Game game = GetGame(context);
			Player player = PlayerStore.GetPlayer(connectionId);

			if (game.CurrentMover != player)
				return;

			PropertyField property = (PropertyField)player.Board.GetFieldById(parsedPacket.FieldId);

			if (property == null || !(property is Site site))
			{
				await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("INTERNAL_ERROR", "Property not found or is not a property that a house can be build on.")));
				return;
			}

			if (site.Owner != player)
			{
				await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("NOT_OWNER", "You don't own this property.")));
				return;
			}

			if (!site.CanBuildHouse(player))
			{
				await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("CANNOT_BUILD", "You cannot build a house on this property.")));
				return;
			}

			if (!player.CanAfford(site.BuildingPrice))
			{
				await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("CANT_AFFORD", "You don't have enough money to buy a house.")));
				return;
			}

			if (site.BuildHouse(player))
			{
				HouseBuiltPacket buildPacket = new HouseBuiltPacket();
				buildPacket.FieldId = site.Id;

				string jsonPacket = JsonSerializer.Serialize(buildPacket);
				await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", jsonPacket);
			}
			else 
			{
				await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("BUILD_FAILED", "Failed to build a house in this property.")));
			}
		}
	}
}
