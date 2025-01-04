using GameServer.Models.Packets;

namespace GameServer.Models
{
	public class Utility : PropertyField
	{
		// depending on dice roll, if one utility is owned -> 4 times the dice 
		// if both are owned -> 10 time the dice 
		//public int[] RentPrices { get; set; }

		public int RolledDice { get; set; }

		public override void LandOn(Player player)
		{
			if (Owner != null && Owner == player)
			{
				//RaiseEvent("INFO", $"{player.Name} ist auf der eigenen Utility gelandet.");
				return;
			}

			if (Owner != null && Owner != player)
			{
				int ownedUtilities = Group.AmountOfWOwnedProperties(Owner);
				int amount = 0;

				if (ownedUtilities == 1)
				{
					amount = RolledDice * 4;
				}
				else if (ownedUtilities == 2) 
				{
					amount = RolledDice * 10;
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

			RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { PlayerName = player.Name, FieldName = this.Name });

		}

		public override void Pass(Player player)
		{
			throw new NotImplementedException();
		}
	}
}
