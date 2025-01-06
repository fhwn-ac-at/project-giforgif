using GameServer.GameLogic;

namespace GameServer.Models.Fields
{
	public class Chance : ActionField
	{
		public override void Accept(IFieldVisitor visitor, Player player, bool isLanding)
		{
			//visitor.Visit(this, player, isLanding);
		}
	}
}
