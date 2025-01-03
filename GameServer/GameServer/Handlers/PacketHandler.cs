﻿using GameServer.Hubs;
using GameServer.Models;
using GameServer.Models.Packets;
using GameServer.Models.Packets.Lobby;
using GameServer.Models.Packets.Rooms;
using GameServer.Stores;
using GameServer.Util;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GameServer.Handlers
{
    public class PacketHandler
    {
        private readonly Dictionary<string, Func<Packet, HubCallerContext, Task>> _packetFunctions = new();
        private readonly IHubContext<Lobby> _lobbyContext;
        private readonly ConnectionMapping _connectionMapping;

        public PacketHandler(IHubContext<Lobby> lobbyContext, ConnectionMapping connectionMapping)
        {
            _lobbyContext = lobbyContext;

            // Hier ein neues packet registrieren
            _packetFunctions.Add("SAMPLE", HandleSamplePacket);
            _packetFunctions.Add("REGISTER", HandleRegisterPacket);
            _packetFunctions.Add("START", HandleStartPacket);
			_packetFunctions.Add("ROLL_DICE", HandleRollDicePacket);
            _packetFunctions.Add("CREATE_ROOM", HandleCreateRoomPacket);
            _packetFunctions.Add("JOIN_ROOM", HandleJoinRoomPacket);
            _packetFunctions.Add("WANT_STATUS", HandleWantStatusPacket);
            _packetFunctions.Add("LEAVE_ROOM", HandleLeaveRoomPacket);
            _connectionMapping = connectionMapping;
        }

        public async Task HandleRegisterPacket(Packet packet, HubCallerContext context)
        {
            RegisterPacket parsed = (RegisterPacket) packet;

            string playerName = parsed.PlayerName ?? GamertagGenerator.GenerateRandomGamertag();
            string connectionId = context.ConnectionId;

            PlayerStore.Add(playerName, connectionId);
        }

        public async Task HandlePacket(Packet packet, HubCallerContext context)
        {
            // checken obs das überhaupt gibt 

            await _packetFunctions[packet.Type](packet, context);
        }

        public async Task HandleStartPacket(Packet packet, HubCallerContext context)
        {
            StartGamePacket parsed = (StartGamePacket) packet;

            Game game = GetGame(context);

            // Determine Player Order
            game.Setup();
        }
        
        public async Task HandleLeaveRoomPacket(Packet packet, HubCallerContext context)
        {
            Game game = GetGame(context);
            Player player = PlayerStore.GetPlayer(context.ConnectionId);

            game.Players.Remove(player);

            PlayerLeftRoomPacket pkg = new PlayerLeftRoomPacket();
            pkg.PlayerName = player.Name;
            string packetJson = JsonSerializer.Serialize(pkg);
            await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson);
            await _lobbyContext.Groups.RemoveFromGroupAsync(context.ConnectionId, GetRoomName(context));
            await SendRoomUpdate(context);
        }

        public async Task HandleCreateRoomPacket(Packet packet, HubCallerContext context)
        {
            CreateRoomPacket parsed = (CreateRoomPacket) packet;
            string connectionId = context.ConnectionId;

            if (string.IsNullOrEmpty(parsed.RoomName))
            {
                await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("EMPTY_NAME", "Please provide a valid room name")));
                return;
            }

            if (RoomStore.Exists(parsed.RoomName))
            {
                await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("ALREADY_TAKEN", "The provided room name is already taken")));
                return;
            }

            if (RoomStore.Add(parsed.RoomName))
            {
                await SendRoomUpdate(context);
                return;
            }

            await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("INTERNAL_ERROR", "Something went wrong while creating the room")));
        }

        private async Task HandleSamplePacket(Packet packet, HubCallerContext context)
        {
            // schauen wer das packet egschickt hat und diesen player getten,
            // Das ist erlaubt weil kein bock auf visitor pattern
            SamplePacket parsed = (SamplePacket) packet;

            Game game = GetGame(context);
            Player player = PlayerStore.GetPlayer(context.ConnectionId);


            // gane. ... einfach drauf reagieren

            RegisterPacket opaket = new RegisterPacket();
            string packetJson = JsonSerializer.Serialize(opaket);
            await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson);
        }

        private async Task HandleWantStatusPacket(Packet packet, HubCallerContext context)
        {
            Player player = PlayerStore.GetPlayer(context.ConnectionId);
            Game game = GetGame(context);
            string roomName = GetRoomName(context);

            StatusPacket pkg = new StatusPacket();
            pkg.Me = player;
            pkg.Players = game.Players.Where(p => p.ConnectionId != player.ConnectionId).ToList();

            string packetJson = JsonSerializer.Serialize(pkg);
            await _lobbyContext.Clients.Client(context.ConnectionId).SendAsync("ReceivePacket", packetJson);
        }

        private async Task HandleJoinRoomPacket(Packet packet, HubCallerContext context)
        {
            JoinRoomPacket parsed = (JoinRoomPacket) packet;

            string connectionId = context.ConnectionId;
            if (!RoomStore.Exists(parsed.RoomName))
            {
                await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("INTERNAL_ERROR", "Something went wrong while creating the room")));
                return;
            }

            Game game = RoomStore.GetGame(parsed.RoomName);

            if (game.Started || game.Players.Count >= 4)
            {
                await _lobbyContext.Clients.Client(connectionId).SendAsync("ReceivePacket", JsonSerializer.Serialize(new ErrorPacket("INTERNAL_ERROR", "Something went wrong while creating the room")));
                return;
            }

            Player player = PlayerStore.GetPlayer(connectionId);
            game.Players.Add(player);
            _connectionMapping.Add(connectionId, parsed.RoomName);
            await _lobbyContext.Groups.AddToGroupAsync(connectionId, parsed.RoomName);


            await SendRoomUpdate(context);
            
            PlayerJoinedPacket pkg = new PlayerJoinedPacket();
            pkg.PlayerName = player.Name;

            string packetJson = JsonSerializer.Serialize(pkg);
            await _lobbyContext.Clients.Group(parsed.RoomName).SendAsync("ReceivePacket", packetJson);
        }

        private async Task SendRoomUpdate(HubCallerContext context)
        {
            var rooms = RoomStore.GetAllRoomNames();

            List<RoomResponse> responses = new List<RoomResponse>();

            foreach (var room in rooms)
            {
                Game game = RoomStore.GetGame(room);
                responses.Add(new RoomResponse(room, game.Started, game.Players.Count));
            }

            RoomsUpdatedPacket packet = new RoomsUpdatedPacket();
            packet.Rooms = responses;
            string packetJson = JsonSerializer.Serialize(packet);
            await _lobbyContext.Clients.All.SendAsync("ReceivePacket", packetJson);
        }
	
		private async Task HandleRollDicePacket(Packet packet, HubCallerContext context)
		{
			RollDicePacket parsedpacket = (RollDicePacket)packet;

			Game game = GetGame(context);

			if (game.CurrentMover == null || game.CurrentMover.Name != parsedpacket.PlayerName)
				return;

			int rolled = game.RollDice();

			RolledPacket rolledPacket = new RolledPacket();
			rolledPacket.PlayerName = parsedpacket.PlayerName;
			rolledPacket.RolledNumber = rolled;

			string packetJson = JsonSerializer.Serialize(rolledPacket);
			await _lobbyContext.Clients.Group(GetRoomName(context)).SendAsync("ReceivePacket", packetJson); 

			// game.continue after roll

			// move player to new field -> and check if passed Go field
			// check on ownership of field
			// if not owned sent packet if they want to buy
			// if owner exists than check field for current rent -> state game -> houses -> rent? -> pay Player2 so und so viel money 

		}



        private string GetRoomName(HubCallerContext context)
        {
            _connectionMapping.TryGetRoomName(context.ConnectionId, out string roomName);
            return roomName;
        }

        private Game GetGame(HubCallerContext context)
        {
            // null checks
            string roomName = GetRoomName(context);
            return RoomStore.GetGame(roomName);
        }
    }
}
