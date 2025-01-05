using GameServer.GameLogic;
namespace GameServer.Models.Fields
{
	public class Station : PropertyField
	{
		// rent for 1, 2, 3 & 4 Stations owned 
		public int[] RentPrices { get; set; } = [];

		public override void Accept(IFieldVisitor visitor, Player player, bool isLanding)
		{
			visitor.Visit(this, player, isLanding);
		}
	}
}
