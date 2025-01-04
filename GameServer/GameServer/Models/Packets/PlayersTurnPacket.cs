namespace GameServer.Models.Packets
{
	public class PlayersTurnPacket : Packet
	{
		public override string Type => "PLAYERS_TURN";

		public string? PlayerName { get; set; }
	}
}
