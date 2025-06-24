using System.Diagnostics;

using RakNet;

namespace SkySaga.Game.Packets;

public static class ClientReadyToPlay
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        Debug.WriteLine("", nameof(ClientReadyToPlay));

        var setClientEntity = new SetClientEntity
        {
            EntityId = connection.Player.Id
        };

        connection.Send(setClientEntity);

        return true;
    }
}