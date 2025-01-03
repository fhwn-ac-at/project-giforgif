namespace GameServer.Models.Packets.Rooms
{
    public class PlayerLeftRoomPacket : Packet
    {
        public override string Type => "PLAYER_LEFT";
        public string? PlayerName { get; set; }
    }
}
