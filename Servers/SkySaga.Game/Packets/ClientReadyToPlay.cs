using System.Diagnostics;

using RakNet;

namespace SkySaga.Game.Packets;

internal static class ClientReadyToPlay
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        Debug.WriteLine("", nameof(ClientReadyToPlay));

        var setClientEntity = new BitStream();

        setClientEntity.WritePacketId(PacketId.SetClientEntity);

        setClientEntity.Write(connection.EntityId); // entityID

        connection.Send(setClientEntity);

        return true;
    }
}