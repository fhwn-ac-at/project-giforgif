namespace GameServer.Models.Packets.Rooms
{
    public class JoinRoomPacket : Packet
    {
        public override string Type => "JOIN_ROOM";
        public string RoomName { get; set; }
    }
}
