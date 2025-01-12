using GameServer.Hubs;
using GameServer.Models.Packets;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using GameServer.Stores;
using GameServer.GameLogic;
using GameServer.Models.Fields;
using System.Numerics;
using GameServer.Models.Packets.Game;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

		private object obj = new object();
		// umschreiben auf schöner
		public async Task HandleReadyPacket(Packet packet, HubCallerContext context)
		{
			Game game = GetGame(context);
			lock (obj)
			{
				if (game.ReadyPlayers == 0)
				{
					game.Setup();
					game.FieldEventOccurred += async (sender, packet) => await Game_FieldEventOccurredAsync(sender, context, packet);
                }


				game.ReadyPlayers++;
			}

			// Integer in Game incrementen

			GameStatePacket pkg = new GameStatePacket();
			pkg.Me = PlayerStore.GetPlayer(context.ConnectionId);
			pkg.Players = game.Players.Where(p => p.ConnectionId != pkg.Me.ConnectionId).ToList();

			string packetJson = JsonSerializer.Serialize(pkg);
			await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", packetJson);


			// when integer is amount of players (3 or 4)
			Console.WriteLine(game.ReadyPlayers);
			if (game.ReadyPlayers >= game.Players.Count)
			{
				new Thread(async () =>
				{
					Thread.Sleep(2000);
					PlayersTurnPacket playersTurn = new PlayersTurnPacket();
					playersTurn.PlayerName = game.CurrentMover.Name;

					packetJson = JsonSerializer.Serialize(playersTurn);
					await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson);
				}).Start();

			}
			// send whos turn it is

		}


		public async Task HandleRollDicePacket(Packet packet, HubCallerContext context)
		{
			RollDicePacket parsedpacket = (RollDicePacket)packet;

			Game game = GetGame(context);

			//game.Callback = (packet) => GameEventOccured(context, packet);

			Player player = PlayerStore.GetPlayer(context.ConnectionId);

			if (game.CurrentMover == null || game.CurrentMover != player)
				return;

			if (player.RoundsLeftInJail > 0)
			{
                
                return;
            }

			if (game.CurrentMoverRolled)
			{
				await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("ALREADY_ROLLED", "You have already rolled the dice.")));
				return;
			}

			// If player has not rolled yet
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
			

			if (e.GetType() == typeof(BuyRequestPacket))
			{
				string packet = JsonSerializer.Serialize(e);
				await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", packet);
				return;
			}

			if (e.GetType() == typeof(GoToJailPacket))
			{
                // First send Package that player is going to jail
                string goToJail = JsonSerializer.Serialize(e, e.GetType());
                await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", goToJail);

				Thread.Sleep(1000);

				// Then send package that next player is up
                Game game = GetGame(context);
				Player newCurrent = game.GetNextPlayer();
				PlayersTurnPacket playersTurn = new PlayersTurnPacket();
				playersTurn.PlayerName = newCurrent.Name;

				string jsonPacket = JsonSerializer.Serialize(playersTurn);
				await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", jsonPacket);
				return;
            }

			if (e.GetType() == typeof(SellPropertiesPacket))
			{
                string packet = JsonSerializer.Serialize(e);
                await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", packet);
                return;
            }

            string pkg = JsonSerializer.Serialize(e, e.GetType());
			await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", pkg);
		}

		public async Task HandlePaymentDecisionPacket(Packet packet, HubCallerContext context)
        {
            PaymentDecisionPacket parsedPacket = (PaymentDecisionPacket)packet;

			Game game = GetGame(context);
            Player player = PlayerStore.GetPlayer(context.ConnectionId);

			var currentMover = game.CurrentMover;

			if (currentMover == null || currentMover != player)
				return;

			if (parsedPacket.WantsToBuy)
			{
				PropertyField? field = currentMover.Board.GetFieldById(currentMover.CurrentPositionFieldId) as PropertyField;

				if (field != null && currentMover.BuyField(field, field.BuyingPrice))
				{
					BoughtFieldPacket buySuccessfulPacket = new BoughtFieldPacket();
					buySuccessfulPacket.PlayerName = currentMover.Name;
					buySuccessfulPacket.FieldId = currentMover.CurrentPositionFieldId;
					buySuccessfulPacket.ReducedBy = field.BuyingPrice;
					
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

			// Player does not want to buy, auction or can't afford 
			StartAuctionPacket auctionPacket = new StartAuctionPacket();
			auctionPacket.FieldId = game.CurrentMover.CurrentPositionFieldId; // Startaution 
            await Console.Out.WriteLineAsync(game.CurrentMover.CurrentPositionFieldId.ToString());

            game.StartAuction(auctionPacket.FieldId);

			string auctionPacketJson = JsonSerializer.Serialize(auctionPacket);
			await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", auctionPacketJson);

		}

		public async Task HandleAuctionBidPacket(Packet packet, HubCallerContext context)
        {
            AuctionBidPacket parsedPacket = (AuctionBidPacket)packet;
			int currentBid = parsedPacket.Bid;
			Player player = PlayerStore.GetPlayer(context.ConnectionId);
			
			Game game = GetGame(context);

			if (game.GetAuctionHighestBidder()?.ConnectionId == player.ConnectionId)
			{
                await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("BID_FAILED", "You are currently the highest bidder.")));
                return;
            }

            var result = false;

            lock (obj)
            {
                result = game.HandleAuctionBid(player, currentBid);
            }

            if (!result)
            {
                await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("BID_FAILED", "You can not afford to bid for this property.")));
                return;
            }

            AuctionBidUpdatePacket auctionBidUpdate = new AuctionBidUpdatePacket();
			auctionBidUpdate.CurrentBid = currentBid;
			auctionBidUpdate.HighestBidderName = player.Name;

			string auctionPacketJson = JsonSerializer.Serialize(auctionBidUpdate);
			await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", auctionPacketJson);
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

		public async Task HandleBuildingPacket(Packet packet, HubCallerContext context)
		{
			BuildHousePacket parsedPacket = (BuildHousePacket)packet;
			string connectionId = context.ConnectionId;

			Game game = GetGame(context);
			Player player = PlayerStore.GetPlayer(connectionId);

			if (game.CurrentMover != player)
				return;

			PropertyField? property = player.Board.GetFieldById(parsedPacket.FieldId) as PropertyField;

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

			if (site.Housecount >= 5)
			{
                await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("MAX_HOUSES", "You can't build more than 5 houses on a property.")));
                return;
            }

			if (!site.CanBuildHouse(player))
			{
				await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("CANNOT_BUILD", "You need to build houses on the other properties first.")));
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
				buildPacket.PlayerName = player.Name;
				buildPacket.Cost = site.BuildingPrice;

                await Console.Out.WriteLineAsync($"Money: {player.Currency.ToString()}");

                string jsonPacket = JsonSerializer.Serialize(buildPacket);
				await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", jsonPacket);
			}
			else 
			{
				await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("BUILD_FAILED", "Failed to build a house in this property.")));
			}
		}

		public async Task HandleEndTurnPacket(Packet packet, HubCallerContext context)
		{
			Player player = PlayerStore.GetPlayer(context.ConnectionId);
			Game game = GetGame(context);

			if (player != game.CurrentMover)
				return;

			if (player.AmountOwed > 0 && player.OwesMoney != null)
			{
                await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("CANT_END_TURN", "You still owe some money.")));
				return;
            }

            Player newCurrent = game.GetNextPlayer();
			PlayersTurnPacket playersTurn = new PlayersTurnPacket();
			playersTurn.PlayerName = newCurrent.Name;


            string jsonPacket = JsonSerializer.Serialize(playersTurn);
			await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", jsonPacket);

            if (game.CurrentMover.RoundsLeftInJail > 0)
			{
				if (game.CurrentMover.RoundsLeftInJail == 1)
				{
					// Last round in jail, has to buyout
					if (newCurrent.CanAfford(50))
					{
						await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", JsonSerializer.Serialize(new JailPayoutSucessPacket() { Cost = 50, PlayerName = newCurrent.Name }));
                        newCurrent.DeductCurrency(50, null);
                        newCurrent.RoundsLeftInJail = 0;
						return;
					}
					else
					{
						await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", JsonSerializer.Serialize(new BankruptcyPacket() { PlayerName = newCurrent.Name }));
						return;
					}
                }

				game.CurrentMover.RoundsLeftInJail--;
            }
		}

		public async Task HandleUseCardPacket(Packet packet, HubCallerContext context)
		{
			UseCardPacket useCardPacket = (UseCardPacket)packet;

			Player player = PlayerStore.GetPlayer(context.ConnectionId);
			Game game = GetGame(context);

			if (player != game.CurrentMover)
				return;

			int cardId = useCardPacket.CardId;

			// get card from cardDealder and invoke action

			// sende to groupe that card was used
		}

        public async Task HandleJailPayoutPacket(Packet packet, HubCallerContext context)
        {
            Player player = PlayerStore.GetPlayer(context.ConnectionId);
			Game game = GetGame(context);

			if (player != game.CurrentMover)
			{
				return;
			}

			if (player.CanAfford(50))
			{
				await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", JsonSerializer.Serialize(new JailPayoutSucessPacket() { Cost = 50, PlayerName = player.Name }));
                player.DeductCurrency(50, null);
                player.RoundsLeftInJail = 0;
			}
			else
			{
				await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", JsonSerializer.Serialize(new BankruptcyPacket() { PlayerName = player.Name}));
			}
        }

        public async Task HandleSellHousePacket(Packet packet, HubCallerContext context)
        {
            Player player = PlayerStore.GetPlayer(context.ConnectionId);
			Game game = GetGame(context);

			if (player != game.CurrentMover)
				return;

			SellHousePacket sellHousePacket = (SellHousePacket)packet;


			IField? field = player.Board.GetFieldById(sellHousePacket.FieldId);
            if (field.GetType() != typeof(Site))
			{
                await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("NO_SITE", "This field does not support houses.")));
            }

			Site site = (Site)field;

			int sellValue = player.SellHouse(site);

            if (sellValue != 0)
			{
                // House sucessfully sold
                await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new HouseSoldPacket() { FieldId = field.Id }));
                await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", JsonSerializer.Serialize(new AddMoneyPacket() { PlayerName = player.Name, Amount = sellValue, Description = "Sold House" }));

				if (player.OwesMoney != null && player.AmountOwed - player.Currency <= 0)
				{
					// Player is currently in debt and Player is out of debt after selling house
					player.TransferCurrency(player.OwesMoney, player.AmountOwed, null);

                    await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", JsonSerializer.Serialize(new PayPlayerPacket() { Amount = player.AmountOwed, From = player.Name, To = player.OwesMoney.Name }));

                    player.AmountOwed = 0;
					player.OwesMoney = null;

                    await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new PlayerOutOfDebt() { }));
					return;
				}
            }
            else
            {
                await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("CANT_SELL", "You cannot sell this House.")));
            }
        }

        public async Task HandleSellPropertyPacket(Packet packet, HubCallerContext context)
        {
            Player player = PlayerStore.GetPlayer(context.ConnectionId);
            Game game = GetGame(context);

            if (player != game.CurrentMover)
                return;

            SellPropertyPacket sellPropPacket = (SellPropertyPacket)packet;

			IField? field = player.Board.GetFieldById(sellPropPacket.FieldId);

            if (field.GetType() != typeof(PropertyField))
            {
                await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("NO_PROPERTY", "This field can not be sold.")));
            }

			int sellValue = player.SellProperty((PropertyField)field);

            if (sellValue != 0)
			{
				await Console.Out.WriteLineAsync($"Sold {player.Name} house for {sellValue}. New Bank account: {player.Currency}");

                // Property sucessfully sold
                await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new PropertySoldPacket() { FieldId = field.Id }));
                await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", JsonSerializer.Serialize(new AddMoneyPacket() { PlayerName = player.Name, Amount = sellValue, Description = "Sold Property" }));

                if (player.OwesMoney != null && player.AmountOwed - player.Currency <= 0)
                {
                    // Player is currently in debt and Player is out of debt after selling property
                    await Console.Out.WriteLineAsync($"Player {player.Name} gets out of debt as he owes {player.AmountOwed} and he has {player.Currency}.");
                    player.TransferCurrency(player.OwesMoney, player.AmountOwed, null);

					await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", JsonSerializer.Serialize(new PayPlayerPacket() { Amount = player.AmountOwed, From = player.Name, To = player.OwesMoney.Name }));

                    player.AmountOwed = 0;
                    player.OwesMoney = null;

                    await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new PlayerOutOfDebt() { }));
					return;
                }
            }
			else
			{
                await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("CANT_SELL", "You cannot sell this Property.")));
            }
        }
    }
}
