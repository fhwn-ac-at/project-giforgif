namespace GameServer.Models.Packets
{
	public class PayPlayerPacket : Packet
	{
		public override string Type => "PAY_PLAYER";

		public Player From { get; set; }

		public Player To { get; set; }

		public int Amount { get; set; }
	}
}
