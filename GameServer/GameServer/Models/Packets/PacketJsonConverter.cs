﻿using System.Text.Json.Serialization;
using System.Text.Json;
using GameServer.Models.Packets.Rooms;
using GameServer.Models.Packets.Lobby;
using GameServer.Models.Packets.Game;
using GameServer.Models.Packets.Rooms.Incoming;

namespace GameServer.Models.Packets
{
    public class PacketJsonConverter : JsonConverter<Packet>
    {
        private readonly Dictionary<string, Type> _packetTypeMappings = new();

        public PacketJsonConverter() {
            _packetTypeMappings.Add("SAMPLE", typeof(SamplePacket));
            _packetTypeMappings.Add("REGISTER", typeof(RegisterPacket));
            _packetTypeMappings.Add("CREATE_ROOM", typeof(CreateRoomPacket));
            _packetTypeMappings.Add("JOIN_ROOM", typeof(JoinRoomPacket));
            _packetTypeMappings.Add("WANT_STATUS", typeof(WantStatusPacket));
            _packetTypeMappings.Add("LEAVE_ROOM", typeof(LeaveRoomPacket));
            _packetTypeMappings.Add("START", typeof(StartGamePacket));
            _packetTypeMappings.Add("ROLL_DICE", typeof(RollDicePacket));
            _packetTypeMappings.Add("PAYMENT_DECISION", typeof(PaymentDecisionPacket));
            _packetTypeMappings.Add("BUILD_HOUSE", typeof(BuildHousePacket));
            _packetTypeMappings.Add("AUCTION_BID", typeof(AuctionBidPacket));
            _packetTypeMappings.Add("END_TURN", typeof(EndTurnPacket));
			_packetTypeMappings.Add("USE_CARD", typeof(UseCardPacket));
            _packetTypeMappings.Add("READY", typeof(ReadyPacket));
            _packetTypeMappings.Add("JAIL_PAYOUT", typeof(JailPayoutPacket));
            _packetTypeMappings.Add("SELL_HOUSE", typeof(SellHousePacket));
            _packetTypeMappings.Add("SELL_PROPERTY", typeof(SellPropertyPacket));
            _packetTypeMappings.Add("WANT_ROOMS", typeof(WantRoomsPacket));
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
