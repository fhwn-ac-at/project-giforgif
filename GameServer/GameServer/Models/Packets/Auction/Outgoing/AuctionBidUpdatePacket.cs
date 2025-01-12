namespace GameServer.Models.Packets
{
	public class AuctionBidUpdatePacket : Packet
	{
		public override string Type => "AUCTION_UPDATE";

		public int CurrentBid { get; set; }

		public string? HighestBidderName { get; set; }
	}
}
