namespace GameServer.Models.Packets
{
	public class HouseBuiltPacket : Packet
	{
		public override string Type => "HOUSE_BUILD";

		public string? PropertyName;
	}
}
