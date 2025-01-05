namespace GameServer.Models.Packets
{
    public class BoughtFieldPacket : Packet
    {
        public override string Type => "BOUGHT_FIELD";
        public string PlayerName { get; set; }
        public int FieldId { get; set; }
    }
}
