namespace GameServer.Models.Packets
{
	public class BuildHousePacket : Packet
	{
		public override string Type => "BUILD_HOUSE";

		public int FieldId { get; set; }
	}
}
