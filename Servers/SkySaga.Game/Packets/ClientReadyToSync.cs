using System.Diagnostics;

using RakNet;

namespace SkySaga.Game.Packets;

public static class ClientReadyToSync
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        Debug.WriteLine("", nameof(ClientReadyToSync));

        connection.InitialChunkSync();

        return true;
    }
}