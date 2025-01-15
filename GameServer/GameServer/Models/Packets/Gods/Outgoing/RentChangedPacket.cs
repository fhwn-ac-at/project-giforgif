namespace GameServer.Models.Packets
{
    public class RentChangedPacket : Packet
    {
        public override string Type => "RENT_INCREASE";
        public int NewMultiplier { get; set; }
    }
}
