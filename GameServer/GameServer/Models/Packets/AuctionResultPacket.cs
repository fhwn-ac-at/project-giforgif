namespace GameServer.Models.Packets
{
    public class AuctionResultPacket : Packet
    {
        public override string Type => "AUCTION_RESULT";

        public string WinnerName { get; set; }
        public int Price { get; set; }
    }
}
