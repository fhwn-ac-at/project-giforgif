namespace GameServer.Models.Packets
{
    public class ErrorPacket : Packet
    {
        public override string Type => "ERROR";
        public string Error { get; set; }
        public string Message { get; set; }

        public ErrorPacket(string error, string message)
        {
            Error = error;
            Message = message;
        }
    }
}
