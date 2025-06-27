using System;
using System.Collections.Generic;

using RakNet;

using SkySaga.Game.Extensions;
using SkySaga.Game.Interfaces;

namespace SkySaga.Game.Packets.Common;

public class ItemSpec : ISerializableType
{
    public int? Unknown;

    public List<int?> Unknown4List = [];
    private const int Unknown4ListDefaultCount = 4;

    public int? Unknown2;

    public string? Unknown3;

    public void Serialize(BitStream bitStream)
    {
        bitStream.WriteOptional(Unknown, bitStream.Write);

        // Count is optimised
        if (Unknown4List.Count < Unknown4ListDefaultCount)
        {
            bitStream.WriteBits(BitConverter.GetBytes(Unknown4List.Count), 32 - Util.NumBitsRequiredUInt32(Unknown4ListDefaultCount), true);
        }
        else
        {
            bitStream.WriteBits(BitConverter.GetBytes(Unknown4ListDefaultCount), 32 - Util.NumBitsRequiredUInt32(Unknown4ListDefaultCount), true);

            bitStream.Write1();
            bitStream.Write(Unknown4List.Count);
        }

        foreach (var unknown4 in Unknown4List)
        {
            bitStream.WriteOptional(unknown4, bitStream.Write);
        }

        bitStream.WriteOptional(Unknown2, bitStream.Write);

        bitStream.WriteString(Unknown3);
    }
}