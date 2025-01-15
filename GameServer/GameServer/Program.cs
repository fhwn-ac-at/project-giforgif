using GameServer.Data;
using GameServer.Handlers;
using GameServer.Hubs;
using GameServer.Stores;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// for localhost
// builder.WebHost.UseUrls("http://0.0.0.0:3000", "https://0.0.0.0:3001");

// for production
builder.WebHost.UseUrls("http://0.0.0.0:3000");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<ConnectionMapping>(); 
builder.Services.AddSingleton<PacketHandler>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200", 
            "http://iscoggo08wk0cswokckckc0w.116.203.80.175.sslip.io", 
            "http://dwcookos0wogw8s80s80scgo.116.203.80.175.sslip.io:9000", 
            "https://api.trucklix.at", 
            "https://stats.trucklix.at", 
            "https://perropoly.trucklix.at")
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
