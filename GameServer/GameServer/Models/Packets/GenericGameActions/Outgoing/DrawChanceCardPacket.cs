namespace GameServer.Models.Packets
{
    public class DrawChanceCardPacket : Packet
    {
        public override string Type => "DRAW_CHANCE";

        public string PlayerName { get; set; }

        public int CardID { get; set; }

        public DrawChanceCardPacket(string playerName, int cardId)
        {
            PlayerName = playerName;
            CardID = cardId;
        }
    }
}
