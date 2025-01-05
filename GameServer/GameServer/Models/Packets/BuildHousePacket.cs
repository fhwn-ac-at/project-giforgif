namespace GameServer.Models.Packets
{
	public class BuildHousePacket : Packet
	{
		public override string Type => "BUILD_HOUSE";

		public string? PropertyName { get; set; }
	}
}
