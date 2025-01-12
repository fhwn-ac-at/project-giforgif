namespace GameServer.Models.Packets.SellPossesions.Outgoing
{
    public class TransferPropertiesPacket : Packet
    {
        public override string Type => "TRANSFER_PROPERTIES";
        public string? From { get; set; }
        public string? To { get; set; }
    }
}
