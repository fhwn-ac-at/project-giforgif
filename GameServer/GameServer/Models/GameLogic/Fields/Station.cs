using GameServer.Models.Packets;

namespace GameServer.Models
{
	public class Station : PropertyField
	{
		// rent for 1, 2, 3 & 4 Stations owned 
		public int[] RentPrices { get; set; } = [];

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
		//		int amount = RentPrices[Group.AmountOfWOwnedProperties(Owner) - 1];

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

		//	RaiseEvent("BUY_REQUEST", new BuyRequestPacket() { PlayerName = player.Name, FieldName = this.Name});
		//}

		//public override void Pass(Player player)
		//{
		//	throw new NotImplementedException();
		//}
	}
}
