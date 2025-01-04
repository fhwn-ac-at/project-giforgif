namespace GameServer.Models.Packets
{
    public class BuyFailedPacket : Packet
    {
        public override string Type => "BUY_FAILED";
        public string PlayerName { get; set; }
        public string PropertyName { get; set; }
    }
}
