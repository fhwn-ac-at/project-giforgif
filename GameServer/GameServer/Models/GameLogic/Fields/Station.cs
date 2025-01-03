using GameServer.Models.Packets;

namespace GameServer.Models
{
	public class Station : PropertyField
	{
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
				int amount = 100;
				player.TransferCurrency(Owner, amount); // TODO: price anpassen und häuser checken und schauen ob player zahlen kann
				RaiseEvent("PAY_PLAYER", new PayPlayerPacket() { From = player, To = Owner, Amount = amount});
			}

			// wilst du zahlen? 
			RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { Player = player, Field = this});
		}

		public override void Pass(Player player)
		{
			throw new NotImplementedException();
		}
	}
}
