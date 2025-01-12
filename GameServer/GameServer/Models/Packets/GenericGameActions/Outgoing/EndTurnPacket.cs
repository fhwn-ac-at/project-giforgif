namespace GameServer.Models.Packets
{
	public class EndTurnPacket : Packet
	{
		public override string Type => "END_TURN";
	}
}
