namespace GameServer.Models.Packets.Game.Outgoing
{
    public class PlayerWonPacket : Packet
    {
        public override string Type => "WON";
        public string? PlayerName { get; set; }
    }
}
