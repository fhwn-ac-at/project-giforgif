namespace GameServer.Models.Packets
{
	public class UseCardPacket : Packet
	{
		public override string Type => "USE_CARD";

		public int CardId { get; set; }
	}
}
