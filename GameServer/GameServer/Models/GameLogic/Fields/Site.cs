using GameServer.Models.Packets;

namespace GameServer.Models
{
	public class Site : PropertyField
	{
		public int Housecount { get; set; }
		// Rent Prices for 0, 1, 2, 3, 4 houses and hotel
		public int[] RentPrices { get; set; }

		public override void LandOn(Player player)
		{
			if (Owner != null && Owner == player)
			{
				//RaiseEvent("INFO", $"{player.Name} ist auf der eigenen Station gelandet.");
				return;
			}

			if (Owner != null && Owner != player)
			{
				int amount = 0;

				if (Housecount > 0) // Ist mind. 1 Haus aus dem Grundstück, bleibt der Preis gleich 
				{
					amount = RentPrices[Housecount];
				}
				else if (Group.AllPropertiesOwnedBy(Owner)) // wenn alle im Besitz sind alle keine Häuser/Hotels vorhanden sind, verdoppelt sich die Rent
				{
					amount = RentPrices[0] * 2;
				}
				else // Gibts keine häuser und nicht alle Grundstücke sind im Besitz
				{
					amount = RentPrices[0]; 
				}

				if (player.TransferCurrency(Owner, amount))
				{
					RaiseEvent("PAY_PLAYER", new PayPlayerPacket() { From = player.Name, To = Owner.Name, Amount = amount, Successful = true });
				}
				else
				{
					RaiseEvent("PAY_PLAYER", new PayPlayerPacket() { From = player.Name, To = Owner.Name, Amount = amount, Successful = false });
					// bancrupcy
				}

				return;
			}

			// wilst du zahlen? 
			RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { PlayerName = player.Name, FieldName = this.Name });

		}

		public override void Pass(Player player)
		{
			throw new NotImplementedException();
		}
	}
}
