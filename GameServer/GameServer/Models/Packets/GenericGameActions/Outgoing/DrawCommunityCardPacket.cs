﻿namespace GameServer.Models.Packets.GenericGameActions.Outgoing
{
    public class DrawCommunityCardPacket : Packet
    {
        public override string Type => "DRAW_CHEST";

        public string PlayerName { get; set; }

        public int CardId { get; set; }

        public DrawCommunityCardPacket(string playerName, int cardId)
        {
            PlayerName = playerName;
            CardId = cardId;
        }
    }
}
