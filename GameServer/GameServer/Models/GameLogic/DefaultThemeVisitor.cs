using GameServer.Models.Packets;
using System.Text.RegularExpressions;

namespace GameServer.Models.GameLogic
{
	public class DefaultThemeVisitor : IFieldVisitor
	{
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
						site.RaiseEvent("BANKRUPTCY", new BankruptcyPacket() { PlayerName = player.Name });
						player.DeclareBankruptcyToPlayer(site.Owner);
					}

					return;
				}

				site.RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { PlayerName = player.Name, FieldName = site.Name });
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
						station.RaiseEvent("BANKRUPTCY", new BankruptcyPacket() { PlayerName = player.Name });
						player.DeclareBankruptcyToPlayer(station.Owner);
					}

					return;
				}

				station.RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { PlayerName = player.Name, FieldName = station.Name });
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
						utility.RaiseEvent("BANKRUPTCY", new BankruptcyPacket() { PlayerName = player.Name });
						player.DeclareBankruptcyToPlayer(utility.Owner);
					}

					return;
				}

				utility.RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { PlayerName = player.Name, FieldName = utility.Name });

			}
			else
			{
				// passing logic 
			}
		}

		public void Visit(CommunityChest communityChest, Player player, bool isLanding)
		{
			// draw from CommunityChest card deck 
			throw new NotImplementedException();
		}

		public void Visit(Chance chance, Player player, bool isLanding)
		{
			// draw from Chance card deck 
			throw new NotImplementedException();
		}

		public void Visit(GoToJail goToJail, Player player, bool isLanding)
		{
			// Send player to jail 
			throw new NotImplementedException();
		}
	}
}
