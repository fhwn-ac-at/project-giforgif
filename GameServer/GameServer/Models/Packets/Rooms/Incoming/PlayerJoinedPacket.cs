namespace GameServer.Models.Packets.Rooms
{
    public class PlayerJoinedPacket : Packet
    {
        public override string Type => "PLAYER_JOINED";
        public string? PlayerName { get; set; }
    }
}
