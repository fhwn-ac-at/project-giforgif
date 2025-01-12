namespace GameServer.Models.Packets.Rooms
{
    public class CreateRoomPacket : Packet
    {
        public override string Type => "CREATE_ROOM";

        public string? RoomName { get; set; }
    }
}
