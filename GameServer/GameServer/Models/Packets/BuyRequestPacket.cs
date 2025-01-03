namespace GameServer.Models.Packets
{
	public class BuyRequestPacket : Packet
	{
		public override string Type => "BUY_REQUEST";

		public Player? Player { get; set; }

		public IField? Field { get; set; }
	}
}
