using System.Diagnostics;

using RakNet;

using SkySaga.Game.Extensions;

namespace SkySaga.Game.Packets;

public static class ClientConnected
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        var clientVersionNumber = bitStream.ReadString();

        Debug.WriteLine($"{nameof(clientVersionNumber)}: {clientVersionNumber}", nameof(ClientConnected));

        var serverInfo = new ServerInfo
        {
            ServerOwnerGuid = "482f2571-e9a6-4f52-97bd-2231a87a9f9a",
            ServerOwnerName = "EDITz",
            ServerBiome = "Desert",
            ServerAdventureCrc = Util.ComputeCrc32("Home_Island_Adventure"),
            IsMyWorld = true,
            ChatHost = "127.0.0.1",
            ChatPort = 444
        };

        connection.Send(serverInfo);

        connection.Send(connection.Map.Definition);

        return true;
    }
}