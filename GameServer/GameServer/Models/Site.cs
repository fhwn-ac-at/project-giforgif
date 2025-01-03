namespace GameServer.Models
{
	public class Site : PropertyField
	{
		public int Housecount { get; set; }
		// Rent Prices for 0, 1, 2, 3, 4 houses and hotel
		public int[] RentPrices { get; set; }

		public override void LandOn(Player player)
		{
			// möglich sein, hat der owner dieser site alle felder von dieser gruppe 
		}

		public override void Pass(Player player)
		{
			throw new NotImplementedException();
		}
	}
}
