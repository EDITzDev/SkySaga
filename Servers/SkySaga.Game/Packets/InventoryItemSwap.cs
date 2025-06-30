using System;
using System.Diagnostics;

using RakNet;

namespace SkySaga.Game.Packets;

public static class InventoryItemSwap
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        if (!bitStream.Read(out int sourceEntityID))
            return false;

        var inOutByteArray = new byte[4];

        if (!bitStream.ReadBits(inOutByteArray, 32 - Util.NumBitsRequiredUInt32(45), true))
            return false;

        var sourceSlotID = BitConverter.ToInt32(inOutByteArray, 0);

        if (!bitStream.Read(out int targetEntityID))
            return false;

        if (!bitStream.ReadBits(inOutByteArray, 32 - Util.NumBitsRequiredUInt32(45), true))
            return false;

        var targetSlotID = BitConverter.ToInt32(inOutByteArray, 0);

        Debug.WriteLine($"sourceEntityID: {sourceEntityID}, sourceSlotID: {sourceSlotID}, targetEntityID: {targetEntityID}, targetSlotID: {targetSlotID}", nameof(InventoryItemSwap));

        return true;
    }
}