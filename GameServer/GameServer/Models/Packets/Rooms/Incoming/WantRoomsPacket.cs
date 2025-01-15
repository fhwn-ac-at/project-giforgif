namespace GameServer.Models.Packets.Rooms.Incoming
{
    public class WantRoomsPacket : Packet
    {
        public override string Type => "WANT_ROOMS";
    }
}
