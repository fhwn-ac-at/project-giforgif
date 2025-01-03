namespace GameServer.Models.Packets
{
	public class RolledPacket : Packet
	{
		public override string Type => "ROLLED";

		public int RolledNumber { get; set; }

		public string? PlayerName { get; set; }
	}
}
