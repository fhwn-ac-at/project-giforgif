namespace GameServer.Models.Packets
{
    public class AuctionResultPacket : Packet
    {
        public override string Type => "AUCTION_RESULT";

        public string PlayerName { get; set; }
        public int Price { get; set; }
    }
}
