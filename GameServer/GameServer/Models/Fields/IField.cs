using GameServer.GameLogic;
using GameServer.Models.Packets;

namespace GameServer.Models.Fields
{
    public interface IField
    {
        int Id { get; set; }
        string Name { get; set; }

        event EventHandler<Packet> FieldEventOccurred;

		void RaiseEvent(string messageType, Packet data);

		// When player lands on the field
		//public void LandOn(Player player);

  //      // When player passes the field
  //      public void Pass(Player player);

        void Accept(IFieldVisitor visitor, Player player, bool isLanding);
    }
}