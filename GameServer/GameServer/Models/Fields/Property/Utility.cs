using GameServer.GameLogic;

namespace GameServer.Models.Fields
{
	public class Utility : PropertyField
	{
		// depending on dice roll, if one utility is owned -> 4 times the dice 
		// if both are owned -> 10 time the dice 
		//public int[] RentPrices { get; set; }

		public int RolledDice { get; set; }

		public override void Accept(IFieldVisitor visitor, Player player, bool isLanding)
		{
			visitor.Visit(this, player, isLanding);
		}

		//public override void LandOn(Player player)
		//{
		//	if (Owner != null && Owner == player)
		//	{
		//		return;
		//	}

		//	if (Owner != null && Owner != player)
		//	{
		//		int ownedUtilities = Group.AmountOfWOwnedProperties(Owner);
		//		int amount = 0;

		//		if (ownedUtilities == 1)
		//		{
		//			amount = RolledDice * 4;
		//		}
		//		else if (ownedUtilities == 2) 
		//		{
		//			amount = RolledDice * 10;
		//		}

		//		if (player.TransferCurrency(Owner, amount))
		//		{
		//			RaiseEvent("PAY_PLAYER", new PayPlayerPacket() { From = player.Name, To = Owner.Name, Amount = amount});
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
