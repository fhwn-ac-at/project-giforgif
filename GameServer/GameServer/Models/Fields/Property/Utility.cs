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
	}
}
