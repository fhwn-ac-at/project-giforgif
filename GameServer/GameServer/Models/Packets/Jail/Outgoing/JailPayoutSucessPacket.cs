namespace GameServer.Models.Packets
{
    public class JailPayoutSucessPacket : Packet
    {
        public override string Type => "PAYOUT_SUCESS";
        public string? PlayerName { get; set; }
        public int Cost { get; set; }

    }
}
