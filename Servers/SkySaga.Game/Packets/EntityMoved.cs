using System;
using System.Numerics;
using System.Diagnostics;

using RakNet;

using SkySaga.Game.Components;

namespace SkySaga.Game.Packets;

public static class EntityMoved
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        if (!bitStream.Read(out int entityId))
            return false;

        var inOutByteArray = new byte[4];

        if (!bitStream.ReadBits(inOutByteArray, 32 - Util.NumBitsRequiredUInt32(0x10000), true))
            return false;

        var positionX = BitConverter.ToInt32(inOutByteArray, 0);

        if (!bitStream.ReadBits(inOutByteArray, 32 - Util.NumBitsRequiredUInt32(0x10000), true))
            return false;

        var positionY = BitConverter.ToInt32(inOutByteArray, 0);

        if (!bitStream.ReadBits(inOutByteArray, 32 - Util.NumBitsRequiredUInt32(0x10000), true))
            return false;

        var positionZ = BitConverter.ToInt32(inOutByteArray, 0);

        if (!bitStream.ReadBits(inOutByteArray, 32 - Util.NumBitsRequiredUInt32(0x6400), true))
            return false;

        var yaw = BitConverter.ToSingle(inOutByteArray, 0);

        Debug.WriteLine($"entityID: {entityId}, position: (x :{positionX} y: {positionY}, z: {positionZ}), yaw: {yaw}", nameof(EntityMoved));

        if (entityId == connection.Player.Id)
        {
            if (connection.Player.TryGetComponent<SmoothedTransformComponent>(out var smoothedTransformComponent))
                smoothedTransformComponent.Position = new Vector<int>([positionX, positionY, positionZ, 0, 0, 0, 0, 0]);
        }

        return true;
    }
}