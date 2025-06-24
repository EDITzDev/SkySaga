using System.Diagnostics;

using RakNet;

namespace SkySaga.Game.Packets;

public static class ClientInitialSyncFinished
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        Debug.WriteLine("", nameof(ClientInitialSyncFinished));

        connection.InitialEntitiySync();

        return true;
    }
}