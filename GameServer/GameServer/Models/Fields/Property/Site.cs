using GameServer.GameLogic;

namespace GameServer.Models.Fields
{
	public class Site : PropertyField
	{
		public int Housecount { get; set; } 

		public int BuildingPrice { get; set; }
		// Rent Prices for 0, 1, 2, 3, 4 houses and hotel
		public int[] RentPrices { get; set; } = [];

		public override void Accept(IFieldVisitor visitor, Player player, bool isLanding)
		{
			visitor.Visit(this, player, isLanding);
		}

		public bool CanBuildHouse(Player player)
		{
			if (Owner != player)
				return false;

			if (!Group.AllPropertiesOwnedBy(player)) 
				return false;

			int maxHousesInGroup = Group.Properties.Max(p => ((Site)p).Housecount);
			int minHousesInGroup = Group.Properties.Min(p => ((Site)p).Housecount);

			if (Housecount > minHousesInGroup)
				return false;

			//if (Housecount >= 4)
				// hotel bauen

			return true;
		}

		public bool BuildHouse(Player player)
		{
			if (!CanBuildHouse(player)) 
				return false;

			player.DeductCurrency(BuildingPrice);
			Housecount++;
			return true;
		}

		//public override void LandOn(Player player)
		//{
		//	if (Owner != null && Owner == player)
		//	{
		//		return;
		//	}

		//	if (Owner != null && Owner != player)
		//	{
		//		int amount = 0;

		//		if (Housecount > 0) // Ist mind. 1 Haus aus dem Grundstück, bleibt der Preis gleich 
		//		{
		//			amount = RentPrices[Housecount];
		//		}
		//		else if (Group.AllPropertiesOwnedBy(Owner)) // wenn alle im Besitz sind alle keine Häuser/Hotels vorhanden sind, verdoppelt sich die Rent
		//		{
		//			amount = RentPrices[0] * 2;
		//		}
		//		else // Gibts keine häuser und nicht alle Grundstücke sind im Besitz
		//		{
		//			amount = RentPrices[0]; 
		//		}

		//		if (player.TransferCurrency(Owner, amount))
		//		{
		//			RaiseEvent("PAY_PLAYER", new PayPlayerPacket() { From = player.Name, To = Owner.Name, Amount = amount });
		//		}
		//		else
		//		{
		//			RaiseEvent("BANKRUPTCY", new BankruptcyPacket() { PlayerName = player.Name });
		//			player.DeclareBankruptcyToPlayer(Owner);
		//		}

		//		return;
		//	}

		//	RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { PlayerName = player.Name, FieldName = this.Name });
		//}

		//public override void Pass(Player player)
		//{
		//	throw new NotImplementedException();
		//}
	}
}
