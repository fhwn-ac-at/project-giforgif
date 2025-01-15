namespace GameServer.Models.Packets
{
    public class PoliticActivatedPacket : Packet
    {
        public override string Type => "NEW_POLITIC";

        public int PoliticId { get; set; }
    }
}
