using System;
using System.Numerics;

using RakNet;

using SkySaga.Game.Extensions;
using SkySaga.Game.Interfaces;

namespace SkySaga.Game.Packets;

public class MapDefinition : ISerializablePacket
{
    /// <summary>
    /// Max 32,32,32
    /// </summary>
    public Vector<int> MapSizeChunks;

    /// <summary>
    /// Crc32 Hash of biome name
    /// </summary>
    public uint? BiomeType;

    /// <summary>
    /// 1 - 
    /// 2 - 
    /// 3 - 
    /// 4 - 
    /// </summary>
    public int GameMode;

    public BitStream Serialize()
    {
        var bitStream = new BitStream();

        bitStream.WritePacketId(PacketId.MapDefinition);

        bitStream.WriteBits(BitConverter.GetBytes(MapSizeChunks[0]), 32 - Util.NumBitsRequiredUInt32(32), true);
        bitStream.WriteBits(BitConverter.GetBytes(MapSizeChunks[1]), 32 - Util.NumBitsRequiredUInt32(32), true);
        bitStream.WriteBits(BitConverter.GetBytes(MapSizeChunks[2]), 32 - Util.NumBitsRequiredUInt32(32), true);

        bitStream.WriteOptional(BiomeType, (value) =>
        {
            bitStream.Write((int)value);
        });

        bitStream.WriteBits(BitConverter.GetBytes(GameMode), 32 - Util.NumBitsRequiredUInt32(4), true);

        return bitStream;
    }
}