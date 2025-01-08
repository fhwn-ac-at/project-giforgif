namespace GameServer.Models.Packets
{
	public class BuyRequestPacket : Packet
	{
		public override string Type => "BUY_REQUEST";

		public int FieldId { get; set; }
	}
}
