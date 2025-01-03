using Newtonsoft.Json;

namespace GameServer.Models.Packets
{
    public abstract class Packet
    {
        [JsonProperty("Type")]
        public abstract string Type { get; }
    }
}
