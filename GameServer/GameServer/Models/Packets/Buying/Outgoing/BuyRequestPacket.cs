namespace GameServer.Models.Packets
{
	public class BuyRequestPacket : Packet
	{
		public override string Type => "BUY_REQUEST";

		public string? PlayerName { get; set; }
		public int FieldId { get; set; }
	}
}
