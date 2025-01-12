namespace GameServer.Models.Packets
{
    public class PaymentDecisionPacket : Packet
    {
        public override string Type => "PAYMENT_DECISION";

        public bool WantsToBuy { get; set; }
    }
}
