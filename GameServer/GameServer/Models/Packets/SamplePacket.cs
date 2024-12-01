namespace GameServer.Models.Packets
{
    public class SamplePacket : Packet
    {
        // Eindeutigen namen vergeben
        public override string Type => "SAMPLE";

        // Zusätzliche Daten hinzufügen
        public string SAMPLE_STRING { get; set; }
        public int SAMPLE_INTEGER { get; set; }
        public int CamelCase { get; set; }
    }
}
