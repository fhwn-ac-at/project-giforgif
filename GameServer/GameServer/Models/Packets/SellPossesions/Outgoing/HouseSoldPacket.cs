namespace GameServer.Models.Packets
{
    public class HouseSoldPacket : Packet
    {
        public override string Type => "HOUSE_SOLD";
        public int FieldId { get; set; }
    }
}
