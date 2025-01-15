using GameServer.Models.Fields;
using GameServer.Models.Packets;
using GameServer.Models.Packets.GenericGameActions.Outgoing;

namespace GameServer.GameLogic
{
	public class DefaultThemeVisitor : IFieldVisitor
	{
		private Game _game;

        public DefaultThemeVisitor(Game game)
        {
            _game = game;
        }

        public void Visit(Site site, Player player, bool isLanding)
		{
			if (site == null)
				return;

			if (isLanding)
			{
				if (site.Owner != null && site.Owner == player)
				{
					return;
				}

				if (site.Owner != null && site.Owner != player)
				{
					int amount = 0;

					if (site.Housecount > 0) // Ist mind. 1 Haus aus dem Grundstück, bleibt der Preis gleich 
					{
						amount = site.RentPrices[site.Housecount];
					}
					else if (site.Group.AllPropertiesOwnedBy(site.Owner)) // wenn alle im Besitz sind alle keine Häuser/Hotels vorhanden sind, verdoppelt sich die Rent
					{
						amount = site.RentPrices[0] * 2;
					}
					else // Gibts keine häuser und nicht alle Grundstücke sind im Besitz
					{
						amount = site.RentPrices[0];
					}

					player.TransferCurrency(site.Owner, amount * site.GodRentModifier, site);

					return;
				}

				site.RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { FieldId = site.Id, PlayerName = player.Name });
			}
		}

		public void Visit(Station station, Player player, bool isLanding)
		{
			if (station == null)
				return;

			if (isLanding)
			{
				if (station.Owner != null && station.Owner == player)
				{
					return;
				}

				if (station.Owner != null && station.Owner != player)
				{
					int amount = station.RentPrices[station.Group.AmountOfWOwnedProperties(station.Owner) - 1];

					player.TransferCurrency(station.Owner, amount * station.GodRentModifier, station);
					return;
				}

				station.RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { FieldId = station.Id, PlayerName = player.Name });
			}
			else 
			{ 
				// Passing Logic 
			}
		}

		public void Visit(Utility utility, Player player, bool isLanding)
		{
			if (utility == null)
				return;

			if (isLanding)
			{
				if (utility.Owner != null && utility.Owner == player)
				{
					return;
				}

				if (utility.Owner != null && utility.Owner != player)
				{
					int ownedUtilities = utility.Group.AmountOfWOwnedProperties(utility.Owner);
					int amount = 0;

					if (ownedUtilities == 1)
					{
						amount = utility.RolledDice * 4;
					}
					else if (ownedUtilities == 2)
					{
						amount = utility.RolledDice * 10;
					}

					player.TransferCurrency(utility.Owner, amount * utility.GodRentModifier, utility);

					return;
				}

				utility.RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { FieldId = utility.Id, PlayerName = player.Name });

			}
			else
			{
				// passing logic 
			}
		}

		public void Visit(CommunityChest communityChest, Player player, bool isLanding)
		{
			if (communityChest == null)
                return;

			if (isLanding)
			{
				var drawnCard = player.Board.Dealer.DrawCommunityCard();

				communityChest.RaiseEvent("DRAW_CHEST", new DrawCommunityCardPacket(player.Name, drawnCard.Id));
				
				if (drawnCard.Effect.IsInstant)
				{
					drawnCard.Effect.Effect.Invoke(player, _game, communityChest);
					return;
				}
			}
		}

        public void Visit(Chance chance, Player player, bool isLanding)
        {
			if (chance == null)
				return;

			if (isLanding)
			{
                var drawnCard = player.Board.Dealer.DrawChanceCard();
                
				chance.RaiseEvent("DRAW_CARD", new DrawChanceCardPacket(player.Name, drawnCard.Id));

                if (drawnCard.Effect.IsInstant)
                {
                    drawnCard.Effect.Effect.Invoke(player, _game, chance);
                    return;
                }
            }
        }

        public void Visit(GoToJail goToJail, Player player, bool isLanding)
		{
			if (goToJail == null)
                return;

			if (isLanding)
			{
				_game.SetPlayerPosition(player, 11, true); // TODO: Find out Jail Field ID; Solved this by hardcoding 11 (Jail Field ID)

				player.RoundsLeftInJail = 3;

				goToJail.RaiseEvent("GO_TO_JAIL", new GoToJailPacket() { PlayerName = player.Name });
			}
		}

        public void Visit(Go go, Player player, bool isLanding)
        {
			if (go == null)
                return;

            player.Currency += 200;

			go.RaiseEvent("PASSED_GO", new AddMoneyPacket() { PlayerName = player.Name, Amount = 200, Description = "Passed Go"});
        }

        public void Visit(Jail jail, Player player, bool isLanding)
        {
            if (jail == null)
                return;

			if (!isLanding)
				return;

			// Maybe add logic here not sure
        }

        public void Visit(FreeParking freeParking, Player player, bool isLanding)
        {
			// Literally nothing happens, maybe event tho idk
			return;
        }

        public void Visit(Tax tax, Player player, bool isLanding)
        {
            if (tax == null)
                return;

			if (isLanding)
			{
                player.DeductCurrency(tax.Amount, tax);
            }
        }
    }
}
