namespace GameServer.Models.Packets
{
    public class GoToJailPacket : Packet
    {
        public override string Type => "GO_TO_JAIL";
        public string? PlayerName { get; set; }
    }
}
