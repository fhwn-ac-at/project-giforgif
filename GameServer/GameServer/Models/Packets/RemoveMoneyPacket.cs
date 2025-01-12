namespace GameServer.Models.Packets
{
    public class RemoveMoneyPacket : Packet
    {
        public override string Type => "REMOVE_MONEY";

        public string? PlayerName { get; set; }
        public int Amount { get; set; }
        public string? Description { get; set; }
    }
}
