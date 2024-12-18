﻿using Microsoft.AspNetCore.SignalR;
using GameServer.Stores;
using GameServer.Models.Packets;
using System.Text.Json;
using GameServer.Handlers;

namespace GameServer.Hubs
{
    public class Lobby : Hub
    {
        private readonly PacketHandler _packetHandler;
        private readonly ConnectionMapping _connectionMapping;

        public Lobby(PacketHandler packetHandler, ConnectionMapping connectionMapping)
        {
            _packetHandler = packetHandler;
            _connectionMapping = connectionMapping;
        }

        public async Task JoinRoom(string roomName)
        {
            if (!RoomStore.Exists(roomName))
            {
                await Clients.Caller.SendAsync("Error", $"Room '{roomName}' does not exist.");
                return;
            }

            // checks machen das nur joinable wenn nicht started und so zeug

            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            // clients informieren das wer gejoined ist
            SamplePacket packet = new SamplePacket();
            packet.SAMPLE_INTEGER = 10;
            packet.CamelCase = 10;
            await Clients.Group(roomName).SendAsync("ReceivePacket", packet);
            _connectionMapping.Add(Context.ConnectionId, roomName);
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ShowWhoLeft", $"{Context.ConnectionId} has left '{roomName}'.");
            _connectionMapping.Remove(Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_connectionMapping.TryGetRoomName(Context.ConnectionId, out string roomName))
            {
                await LeaveRoom(roomName);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendPacketToServer(string packetJson)
        {
            var packet = JsonSerializer.Deserialize<Packet>(packetJson, new JsonSerializerOptions
            {
                Converters = { new PacketJsonConverter() },
                PropertyNameCaseInsensitive = true
            });

            if (packet != null)
            {
                await _packetHandler.HandlePacket(packet, Context);
                return;
            }

            await Clients.Caller.SendAsync("Error", "Ungültiges Paketformat.");

        }

        public async Task SendPacketToRoom(string roomName, Packet packet)
        {
            string packetJson = JsonSerializer.Serialize(packet);
            await Clients.Group(roomName).SendAsync("ReceivePacket", packetJson);
        }
    }
}
