namespace GameServer.Models.Packets
{
    public class SellHousePacket : Packet
    {
        public override string Type => "SELL_HOUSE";

        public int FieldId { get; set; }
    }
}
