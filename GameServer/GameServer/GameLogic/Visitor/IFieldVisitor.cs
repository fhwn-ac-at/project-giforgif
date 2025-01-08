using GameServer.Models.Fields;

namespace GameServer.GameLogic
{
	public interface IFieldVisitor
	{
		void Visit(Site site, Player player, bool isLanding);
		void Visit(Station station, Player player, bool isLanding);
		void Visit(Utility utility, Player player, bool isLanding);
		void Visit(CommunityChest communityChest, Player player, bool isLanding);
		void Visit(GoToJail goToJail, Player player, bool isLanding);
		void Visit(Go go, Player player, bool isLanding);
		void Visit(Jail jail, Player player, bool isLanding);
		void Visit(FreeParking freeParking, Player player, bool isLanding);
		void Visit(Tax tax, Player player, bool isLanding);
	}
}
