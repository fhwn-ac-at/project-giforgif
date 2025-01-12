namespace GameServer.Models.Packets
{
    public class SellPropertiesPacket : Packet
    {
        public override string Type => "SELL_PROPERTIES";
        public int Amount { get; set; } // Amount of money player has to sell in order to pay
    }
}
