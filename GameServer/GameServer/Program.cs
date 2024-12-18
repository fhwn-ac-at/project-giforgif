using GameServer.Handlers;
using GameServer.Hubs;
using GameServer.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<ConnectionMapping>();
builder.Services.AddSingleton<PacketHandler>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});



var app = builder.Build();


app.MapControllers();
app.MapHub<Lobby>("/lobby");

app.UseCors("AllowAllOrigins");

app.Run();
