using System;

using RakNet;

using SkySaga.Game.Extensions;
using SkySaga.Game.Interfaces;

namespace SkySaga.Game.Packets.Common;

public class InventorySlotData : ISerializableType
{
    /// <remarks>
    /// <c>GeoData.json > Resources > Name</c>
    /// </remarks>
    public uint? Name;

    public int Count;

    public bool Unknown3;

    public int Unknown4;

    public int Unknown5;

    public string? ItemUUID;

    public void Serialize(BitStream bitStream)
    {
        bitStream.WriteOptional(Name, (value) =>
        {
            bitStream.Write((int)value);
        });

        bitStream.Write(Count > 64);
        bitStream.WriteBits(BitConverter.GetBytes(Count), 32 - Util.NumBitsRequiredUInt32((Count > 64 ? 0x10000u : 64u)), true);

        bitStream.Write(Unknown3);

        bitStream.Write(Unknown4 > 64);
        bitStream.WriteBits(BitConverter.GetBytes(Unknown4), 32 - Util.NumBitsRequiredUInt32((Unknown4 > 64 ? 0x10000u : 64u)), true);

        bitStream.WriteBits(BitConverter.GetBytes(Unknown5), 32 - Util.NumBitsRequiredUInt32(0x10000), true);

        bitStream.WriteString(ItemUUID);
    }
}