using GameServer.Models.Packets;

namespace GameServer.Models
{
	public class Station : PropertyField
	{
		// rent for 1, 2, 3 & 4 Stations owned 
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
				int amount = RentPrices[Group.AmountOfWOwnedProperties(Owner) - 1];

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
			RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { PlayerName = player.Name, FieldName = this.Name});
		}

		public override void Pass(Player player)
		{
			throw new NotImplementedException();
		}
	}
}
