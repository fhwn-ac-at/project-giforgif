using GameServer.Models.Fields;

namespace GameServer.GameLogic
{
	public interface IFieldVisitor
	{
		void Visit(Site site, Player player, bool isLanding);
		void Visit(Station station, Player player, bool isLanding);
		void Visit(Utility utility, Player player, bool isLanding);
		void Visit(CommunityChest communityChest, Player player, bool isLanding);
		void Visit(Chance chance, Player player, bool isLanding);
		void Visit(GoToJail goToJail, Player player, bool isLanding);
	}
}
