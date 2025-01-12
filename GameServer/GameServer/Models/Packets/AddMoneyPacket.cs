namespace GameServer.Models.Packets
{
    public class AddMoneyPacket : Packet
    {
        public override string Type => "ADD_MONEY";

        public string? PlayerName { get; set; }
        public int Amount { get; set; }
        public string? Description { get; set; }
    }
}
