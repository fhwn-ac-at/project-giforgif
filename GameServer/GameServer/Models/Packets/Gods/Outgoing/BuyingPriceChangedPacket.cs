namespace GameServer.Models.Packets
{
    public class BuyingPriceChangedPacket : Packet
    {
        public override string Type => "BUYING_PRICE_INCREASE";

        public int NewMultiplier { get; set; }
    }
}
