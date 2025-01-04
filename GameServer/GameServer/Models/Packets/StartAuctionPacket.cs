namespace GameServer.Models.Packets
{
    public class StartAuctionPacket : Packet
    {
        public override string Type => "AUCTION_START";

        public string PropertyName { get; set; }
    }
}
