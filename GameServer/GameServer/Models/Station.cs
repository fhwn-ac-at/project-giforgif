namespace GameServer.Models
{
	public class Station : PropertyField
	{
		public int[] RentPrices { get; set; }

		public override void LandOn(Player player)
		{
			if (Owner != null && Owner == player)
			{
				return;
			}

			if (Owner != null && Owner != player) 
			{
				int amount = 100;
				player.TransferCurrency(Owner, amount); // TODO: price anpassen und häuser checken und schauen ob player zahlen kann
			}

			// wilst du zahlen? 

		}

		public override void Pass(Player player)
		{
			throw new NotImplementedException();
		}
	}
}
