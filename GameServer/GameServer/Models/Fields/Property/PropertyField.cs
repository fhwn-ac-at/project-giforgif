using GameServer.GameLogic;
using GameServer.Models.Packets;

namespace GameServer.Models.Fields
{
	public abstract class PropertyField : IField
	{
        public Player? Owner { get; set; }

        public string Name { get; set; }

        public int BuyingPrice { get; set; }

		public PropertyGroup? Group { get; set; } // muss halt noch befüllt werden
		public int Id { get; set; }

		public int GodRentModifier { get; set; } = 1;

		public event EventHandler<Packet>? FieldEventOccurred;

		public abstract void Accept(IFieldVisitor visitor, Player player, bool isLanding);

		//public abstract void LandOn(Player player);

		//public abstract void Pass(Player player);

		// hier könnte der MessageType weg genommen werden, da nix damit passiert, aber bleibt noch zur übersicht
		public void RaiseEvent(string messageType, Packet packet)
		{
			FieldEventOccurred?.Invoke(this, packet);
		}
	}
}
