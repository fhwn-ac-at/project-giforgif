namespace GameServer.Models.Packets
{
    public class AuctionBidPacket : Packet
    {
        public override string Type => "AUCTION_BID";

        //public string PlayerName { get; set; }
        public int Bid { get; set; }
    }
}
