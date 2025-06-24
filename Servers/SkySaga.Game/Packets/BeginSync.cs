using System;

using RakNet;

using SkySaga.Game.Extensions;
using SkySaga.Game.Interfaces;

namespace SkySaga.Game.Packets;

public class BeginSync : ISerializablePacket
{
    public int NumChunksToSync;

    public BitStream Serialize()
    {
        var bitStream = new BitStream();

        bitStream.WritePacketId(PacketId.BeginSync);

        bitStream.WriteBits(BitConverter.GetBytes(NumChunksToSync), 32 - Util.NumBitsRequiredUInt32(0x8000u), true);

        return bitStream;
    }
}