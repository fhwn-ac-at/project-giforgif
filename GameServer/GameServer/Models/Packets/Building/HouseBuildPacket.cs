namespace GameServer.Models.Packets
{
	public class HouseBuiltPacket : Packet
	{
		public override string Type => "HOUSE_BUILT";
		public int? FieldId { get; set; }
		public int? Cost { get; set; }
	}
}
