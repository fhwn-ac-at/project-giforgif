﻿using GameServer.Models.Fields;
using GameServer.Models.Packets;

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

					if (player.TransferCurrency(site.Owner, amount))
					{
						site.RaiseEvent("PAY_PLAYER", new PayPlayerPacket() { From = player.Name, To = site.Owner.Name, Amount = amount });
					}
					else
					{
                        Console.WriteLine($"Player {player.Name} can not pay the amount of {amount}. His networth is {player.CalculateNetWorth()}");

                        if (player.CalculateNetWorth() < amount)
						{
							// If Player is not able to pay the owned rent
							player.DeclareBankruptcyToPlayer(site.Owner);
							site.RaiseEvent("BANKRUPTCY", new BankruptcyPacket() { PlayerName = player.Name });

                            Console.WriteLine($"Player {player.Name} declared Bankrupcty to {site.Owner.Name}. All Properties got transferred.");
							// May need to add something to count player out of the game
                            return;
						}

						//Otherwise player can still sell properties to pay the rent
						player.OwesMoney = site.Owner;
						player.AmountOwed = amount;
						site.RaiseEvent("SELL_PROPERTIES", new SellPropertiesPacket() {Amount = amount - player.Currency});
					}

					return;
				}

				site.RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { FieldId = site.Id });
			}
			else
			{
				// passing logic 
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

					if (player.TransferCurrency(station.Owner, amount))
					{
						station.RaiseEvent("PAY_PLAYER", new PayPlayerPacket() { From = player.Name, To = station.Owner.Name, Amount = amount });
					}
					else
					{
                        Console.WriteLine($"Player {player.Name} can not pay the amount of {amount}. His networth is {player.CalculateNetWorth()}");

                        if (player.CalculateNetWorth() < amount)
                        {
                            // If Player is not able to pay the owned rent
                            player.DeclareBankruptcyToPlayer(station.Owner);
                            station.RaiseEvent("BANKRUPTCY", new BankruptcyPacket() { PlayerName = player.Name });

                            Console.WriteLine($"Player {player.Name} declared Bankrupcty to {station.Owner.Name}. All Properties got transferred.");
                            return;
                        }

                        //Otherwise player can still sell properties to pay the rent
                        player.OwesMoney = station.Owner;
                        player.AmountOwed = amount;
                        station.RaiseEvent("SELL_PROPERTIES", new SellPropertiesPacket() {Amount = amount - player.Currency });
                    }

					return;
				}

				station.RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { FieldId = station.Id });
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

					if (player.TransferCurrency(utility.Owner, amount))
					{
						utility.RaiseEvent("PAY_PLAYER", new PayPlayerPacket() { From = player.Name, To = utility.Owner.Name, Amount = amount });
					}
					else
					{
                        Console.WriteLine($"Player {player.Name} can not pay the amount of {amount}. His networth is {player.CalculateNetWorth()}");

                        if (player.CalculateNetWorth() < amount)
                        {
                            // If Player is not able to pay the owned rent
                            player.DeclareBankruptcyToPlayer(utility.Owner);
                            utility.RaiseEvent("BANKRUPTCY", new BankruptcyPacket() { PlayerName = player.Name });

                            Console.WriteLine($"Player {player.Name} declared Bankrupcty to {utility.Owner.Name}. All Properties got transferred.");
                            return;
                        }

                        //Otherwise player can still sell properties to pay the rent
                        player.OwesMoney = utility.Owner;
                        player.AmountOwed = amount;
                        utility.RaiseEvent("SELL_PROPERTIES", new SellPropertiesPacket() {Amount = amount - player.Currency });
                    }

					return;
				}

				utility.RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { FieldId = utility.Id });

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
				var drawnCard = player.Board.DrawCard();

				if (drawnCard.Effect.IsInstant)
				{
					drawnCard.Effect.Effect.Invoke(player);
					return;
				}

				communityChest.RaiseEvent("DRAW_CARD", new DrawCardPacket(player.Name, drawnCard.Name, drawnCard.Description));
			}
		}

		public void Visit(GoToJail goToJail, Player player, bool isLanding)
		{
			if (goToJail == null)
                return;

			if (isLanding)
			{
				_game.SetPlayerPosition(player, 11); // TODO: Find out Jail Field ID; Solved this by hardcoding 11 (Jail Field ID)

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
				player.DeductCurrency(tax.Amount);

				tax.RaiseEvent("TAX_PAY", new RemoveMoneyPacket() { PlayerName = player.Name, Amount = tax.Amount, Description = "Landed on tax field"});
			}
        }
    }
}
