namespace GameServer.Models
{
	public class Utility : PropertyField
	{
		// depending on dice roll, if one utility is owned -> 4 times the dice 
		// if both are owned -> 10 time the dice 
		//public int[] RentPrices { get; set; }

		public override void LandOn(Player player)
		{
			throw new NotImplementedException();
		}

		public override void Pass(Player player)
		{
			throw new NotImplementedException();
		}
	}
}
