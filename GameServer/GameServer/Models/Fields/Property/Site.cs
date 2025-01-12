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

			player.DeductCurrency(BuildingPrice, null);
			Housecount++;
			return true;
		}
	}
}
