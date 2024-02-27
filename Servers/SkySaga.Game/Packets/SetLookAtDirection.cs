using System;
using System.Diagnostics;

using RakNet;

namespace SkySaga.Game.Packets;

internal static class SetLookAtDirection
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        var inOutByteArray = new byte[4];

        if (!bitStream.ReadBits(inOutByteArray, 32 - Util.NumBitsRequiredUInt32(4), true))
            return false;

        var lookAtMode = BitConverter.ToInt32(inOutByteArray, 0);

        if (!bitStream.ReadBits(inOutByteArray, 32 - Util.NumBitsRequiredUInt32(0x6400), true))
            return false;

        var pitch = BitConverter.ToSingle(inOutByteArray, 0);

        if (!bitStream.ReadBits(inOutByteArray, 32 - Util.NumBitsRequiredUInt32(0x6400), true))
            return false;

        var yaw = BitConverter.ToSingle(inOutByteArray, 0);

        Debug.WriteLine($"lookAtMode: {lookAtMode}, pitch: {pitch}, yaw: {yaw}", nameof(SetLookAtDirection));

        return true;
    }
}