namespace GameServer.Models.Packets
{
    public class PropertySoldPacket : Packet
    {
        public override string Type => "PROPERTY_SOLD";
        public int FieldId { get; set; }
    }
}
