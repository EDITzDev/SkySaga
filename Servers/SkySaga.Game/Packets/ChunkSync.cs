using System;
using System.Numerics;

using RakNet;

using SkySaga.Game.Extensions;
using SkySaga.Game.Interfaces;

namespace SkySaga.Game.Packets;

public class ChunkSync : ISerializablePacket
{
    public Vector<int> Coords;

    public byte[]? Data1;
    public byte[]? Data2;

    public byte? AdjacentChunks;

    public BitStream Serialize()
    {
        var bitStream = new BitStream();

        bitStream.WritePacketId(PacketId.ChunkSync);

        bitStream.WriteBits(BitConverter.GetBytes(Coords[0]), 32 - Util.NumBitsRequiredUInt32(32), true);
        bitStream.WriteBits(BitConverter.GetBytes(Coords[1]), 32 - Util.NumBitsRequiredUInt32(32), true);
        bitStream.WriteBits(BitConverter.GetBytes(Coords[2]), 32 - Util.NumBitsRequiredUInt32(32), true);

        bitStream.WriteOptional(Data1, (value) =>
        {
            bitStream.Write(value.Length);
            bitStream.WriteAlignedBytes(value, (uint)value.Length);
        });

        bitStream.WriteOptional(Data2, (value) =>
        {
            bitStream.Write(value.Length);
            bitStream.WriteAlignedBytes(value, (uint)value.Length);
        });

        bitStream.WriteOptional(AdjacentChunks, (value) =>
        {
            bitStream.WriteBits([value], 8 - Util.NumBitsRequiredByte(64), true);
        });

        return bitStream;
    }
}