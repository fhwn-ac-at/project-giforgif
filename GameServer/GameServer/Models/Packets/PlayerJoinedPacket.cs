namespace GameServer.Models.Packets
{
    public class PlayerJoinedPacket : Packet
    {
        public override string Type => "PLAYER_JOINED";
        public string? PlayerName { get; set; }

    }
}
