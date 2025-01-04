namespace GameServer.Models.Packets
{
    public class BuySuccessfulPacket : Packet
    {
        public override string Type => "BUY_SUCESS";
        public string PlayerName { get; set; }
        public string PropertyName { get; set; }
    }
}
