namespace GameServer.Models.Packets
{
    public class MovePlayerPacket : Packet
    {
        public override string Type => "MOVE_PLAYER";
        public string? PlayerName { get; set; }
        public int FieldId { get; set; }
    }
}
