namespace GameServer.Models.Packets
{
	public class RollDicePacket : Packet
	{
		public override string Type => "ROLL_DICE";

		public string? PlayerName { get; set; }
	}
}
