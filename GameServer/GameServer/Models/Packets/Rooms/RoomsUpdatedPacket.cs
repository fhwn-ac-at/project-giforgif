namespace GameServer.Models.Packets.Rooms
{
    public class RoomsUpdatedPacket : Packet
    {
        public override string Type => "ROOMS_UPDATED";
        public List<RoomResponse> Rooms { get; set; }
    }
}
