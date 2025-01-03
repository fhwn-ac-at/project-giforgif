namespace GameServer.Models
{
	public class Station : PropertyField
	{
		public int[] RentPrices { get; set; }

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
