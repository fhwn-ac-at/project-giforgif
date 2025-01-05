namespace GameServer.Models.Packets
{
	public class BuyHousePacket : Packet
	{
		public override string Type => "BUY_HOUSE";

		public string? PropertyName { get; set; }
	}
}
