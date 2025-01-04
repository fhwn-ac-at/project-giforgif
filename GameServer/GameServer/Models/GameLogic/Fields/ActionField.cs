using GameServer.Models.Packets;

namespace GameServer.Models
{
    public class ActionField : IField
    {
        public string Name { get; set; }

        public event EventHandler<Packet> FieldEventOccurred;

        public void LandOn(Player player)
        {
            throw new NotImplementedException();
        }

        public void Pass(Player player)
        {
            throw new NotImplementedException();
        }

        public void RaiseEvent(string messageType, Packet data)
        {
            throw new NotImplementedException();
        }
    }
}
