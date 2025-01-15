namespace GameServer.Models.Packets
{
    public class DrawCardPacket : Packet
    {
        public override string Type => "DRAW_CARD";

        public string PlayerName { get; set; }

        public int CardID { get; set; }

        public DrawCardPacket(string playerName, int cardId)
        {
            PlayerName = playerName;
            CardID = cardId;
        }
    }
}
