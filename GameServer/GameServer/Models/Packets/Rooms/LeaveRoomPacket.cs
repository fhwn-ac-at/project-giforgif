namespace GameServer.Models.Packets.Rooms
{
    public class LeaveRoomPacket : Packet
    {
        public override string Type => "LEAVE_ROOM";
    }
}
