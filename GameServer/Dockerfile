# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 3000
EXPOSE 3001

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything into the container's build context
COPY . .

# Restore dependencies
RUN dotnet restore "GameServer/GameServer.csproj"

# Build the application
WORKDIR "/src/GameServer"
RUN dotnet build "GameServer.csproj" -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "GameServer.csproj" -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point to the application
ENTRYPOINT ["dotnet", "GameServer.dll"]
