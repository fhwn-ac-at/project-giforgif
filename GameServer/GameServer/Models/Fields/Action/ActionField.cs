using GameServer.GameLogic;
using GameServer.Models.Packets;

namespace GameServer.Models.Fields
{
    public abstract class ActionField : IField
    {
        public string Name { get; set; }
		public int Id { get; set; }

		public event EventHandler<Packet>? FieldEventOccurred;

        public abstract void Accept(IFieldVisitor visitor, Player player, bool isLanding);

        public void RaiseEvent(string messageType, Packet data)
        {
			FieldEventOccurred?.Invoke(this, data);
		}
    }
}
