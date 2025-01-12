namespace GameServer.Models.Packets
{
    public class SellPropertyPacket : Packet
    {
        public override string Type => "SELL_PROPERTY";
        public int FieldId { get; set; }
    }
}
