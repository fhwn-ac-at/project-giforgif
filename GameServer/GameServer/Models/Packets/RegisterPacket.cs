namespace GameServer.Models.Packets
{
    public class RegisterPacket : Packet
    {
        public override string Type => "REGISTER";
        public string? PlayerName { get; set; }

    }
}
