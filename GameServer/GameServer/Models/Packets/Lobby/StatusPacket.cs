using GameServer.GameLogic;

namespace GameServer.Models.Packets.Lobby
{
    public class StatusPacket : Packet
    {
        public override string Type => "STATUS";

        public Player Me { get; set; }
        public List<Player> Players { get; set; }
    }
}
