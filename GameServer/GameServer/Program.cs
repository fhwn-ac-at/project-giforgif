using GameServer.Handlers;
using GameServer.Hubs;
using GameServer.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<ConnectionMapping>();
builder.Services.AddSingleton<PacketHandler>();

var app = builder.Build();

app.MapControllers();
app.MapHub<Lobby>("/lobby");

app.Run();
