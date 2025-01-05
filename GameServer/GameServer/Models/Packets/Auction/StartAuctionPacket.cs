namespace GameServer.Models.Packets
{
    public class StartAuctionPacket : Packet
    {
        public override string Type => "AUCTION_START";

        public int FieldId { get; set; }
    }
}
