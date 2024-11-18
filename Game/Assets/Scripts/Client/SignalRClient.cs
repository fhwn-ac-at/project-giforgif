using System;
using System.Threading.Tasks;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;

public class SignalRClient : MonoBehaviour
{
    private HubConnection connection;

    async void Start()
    {
        await ConnectToHub();
    }

    private async Task ConnectToHub()
    {
        connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:32769/lobby")
            .Build();

        connection.On<string>("ReceivePacket", (packetJson) =>
        {
            Debug.Log($"Received packet: {packetJson}");
        });


        try
        {
            await connection.StartAsync();
            Debug.Log("Connected to SignalR hub.");

            // Optionally, join a room or perform other initial actions
            await JoinRoom("hallo", "PlayerName");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error connecting to SignalR hub: {ex.Message}");
        }
    }

    public async Task JoinRoom(string roomName, string playerName)
    {
        if (connection.State == HubConnectionState.Connected)
        {
            try
            {
                await connection.InvokeAsync("JoinRoom", roomName);
                Debug.Log($"Joined room: {roomName} as {playerName}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error joining room: {ex.Message}");
            }
        }
    }

    public async Task LeaveRoom(string roomName)
    {
        if (connection.State == HubConnectionState.Connected)
        {
            try
            {
                await connection.InvokeAsync("LeaveRoom", roomName);
                Debug.Log($"Left room: {roomName}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error leaving room: {ex.Message}");
            }
        }
    }

    public async Task SendPacketToServer(object packet)
    {
        if (connection.State == HubConnectionState.Connected)
        {
            try
            {
                string packetJson = JsonUtility.ToJson(packet);
                await connection.InvokeAsync("SendPacketToServer", packetJson);
                Debug.Log($"Sent packet to server: {packetJson}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error sending packet: {ex.Message}");
            }
        }
    }

    private async void OnApplicationQuit()
    {
        if (connection != null)
        {
            await connection.StopAsync();
            await connection.DisposeAsync();
            Debug.Log("Disconnected from SignalR hub.");
        }
    }
}
