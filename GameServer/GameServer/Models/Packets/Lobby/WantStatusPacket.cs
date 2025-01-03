namespace GameServer.Models.Packets.Lobby
{
    public class WantStatusPacket : Packet
    {
        public override string Type => "WANT_STATUS";
    }
}
