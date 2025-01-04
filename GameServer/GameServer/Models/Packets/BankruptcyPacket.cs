namespace GameServer.Models.Packets
{
	public class BankruptcyPacket : Packet
	{
		public override string Type => "BANKRUPTCY";

		public string? PlayerName { get; set; }
	}
}
