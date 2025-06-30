using System;
using System.Collections.Generic;

using RakNet;

using SkySaga.Game.Extensions;
using SkySaga.Game.Interfaces;

namespace SkySaga.Game.Packets.Common;

public class ItemSpec : ISerializableType
{
    /// <remarks>
    /// <c>GeoData.json > Resources > Name</c>
    /// </remarks>
    public uint? NameHash;

    public List<int?> MaterialList = [0, 0, 0, 0];
    private const int MaterialListDefaultCount = 4;

    public uint? TeachItemCRC;

    public string? UUID;

    public static ItemSpec Empty = new ItemSpec();

    public void Serialize(BitStream bitStream)
    {
        bitStream.WriteOptional(NameHash, (value) =>
        {
            bitStream.Write((int)value);
        });

        // Count is optimised
        if (MaterialList.Count < MaterialListDefaultCount)
        {
            bitStream.WriteBits(BitConverter.GetBytes(MaterialList.Count), 32 - Util.NumBitsRequiredUInt32(MaterialListDefaultCount), true);
        }
        else
        {
            bitStream.WriteBits(BitConverter.GetBytes(MaterialListDefaultCount), 32 - Util.NumBitsRequiredUInt32(MaterialListDefaultCount), true);

            bitStream.Write1();
            bitStream.Write(MaterialList.Count);
        }

        foreach (var unknown4 in MaterialList)
        {
            bitStream.WriteOptional(unknown4, bitStream.Write);
        }

        bitStream.WriteOptional(TeachItemCRC, (value) =>
        {
            bitStream.Write((int)value);
        });

        bitStream.WriteString(UUID);
    }
}