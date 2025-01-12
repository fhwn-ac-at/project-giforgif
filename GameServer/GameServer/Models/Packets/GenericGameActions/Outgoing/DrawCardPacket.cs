namespace GameServer.Models.Packets
{
    public class DrawCardPacket : Packet
    {
        public override string Type => "DRAW_CARD";

        public string PlayerName { get; set; }

        public string CardName { get; set; }

        public string CardDescription { get; set; }
        public DrawCardPacket(string playerName, string cardName, string cardDescription)
        {
            PlayerName = playerName;
            CardName = cardName;
            CardDescription = cardDescription;
        }
    }
}
