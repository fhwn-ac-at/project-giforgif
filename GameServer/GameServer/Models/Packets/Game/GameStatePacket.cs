using GameServer.GameLogic;

namespace GameServer.Models.Packets.Game
{
    public class GameStatePacket : Packet
    {
        public override string Type => "GAME_STATE";
        public Player Me { get; set; }
        public List<Player> Players { get; set; }
    }
}
