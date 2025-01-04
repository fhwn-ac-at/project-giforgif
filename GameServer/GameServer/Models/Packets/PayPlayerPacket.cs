namespace GameServer.Models.Packets
{
	public class PayPlayerPacket : Packet
	{
		public override string Type => "PAY_PLAYER";

		public string From { get; set; }

		public string To { get; set; }

		public int Amount { get; set; }
	}
}
