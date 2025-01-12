namespace GameServer.Models.Packets
{
    public class AuctionResultPacket : Packet
    {
        public override string Type => "AUCTION_RESULT";

        public string? WinnerPlayerName { get; set; }
        public int WinningBid { get; set; } 
        public int PropertyId { get; set; }
    }
}
