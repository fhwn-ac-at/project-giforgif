using System.Text.Json.Serialization;
using System.Text.Json;

namespace GameServer.Models.Packets
{
    public class PacketJsonConverter : JsonConverter<Packet>
    {
        private readonly Dictionary<string, Type> _packetTypeMappings = new();

        public PacketJsonConverter() {
            _packetTypeMappings.Add("SAMPLE", typeof(SamplePacket));
            _packetTypeMappings.Add("REGISTER", typeof(RegisterPacket));
            _packetTypeMappings.Add("ROLL_DICE", typeof(RollDicePacket));
        }

        public override Packet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            if (document.RootElement.TryGetProperty("Type", out JsonElement typeElement))
            {
                var type = typeElement.GetString();

                if (type != null && _packetTypeMappings.TryGetValue(type, out Type? packetType))
                {
                    return (Packet) document.Deserialize(packetType, options);
                }
                
                throw new JsonException($"Unbekannter Pakettyp: {type}");
                
            }

            throw new JsonException("Unbekannter Pakettyp");
        }

        public override void Write(Utf8JsonWriter writer, Packet value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}
