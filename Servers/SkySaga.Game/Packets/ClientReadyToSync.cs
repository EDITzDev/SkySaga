using System;
using System.Diagnostics;

using RakNet;

namespace SkySaga.Game.Packets;

internal static class ClientReadyToSync
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        Debug.WriteLine("", nameof(ClientReadyToSync));

        var beginSync = new BitStream();

        beginSync.WritePacketId(PacketId.BeginSync);

        beginSync.WriteBits(BitConverter.GetBytes(4u), 32 - Util.NumBitsRequiredUInt32(0x8000u), true); // numChunksToSync

        connection.Send(beginSync);

        {
            var chunkSync = new BitStream();

            chunkSync.WritePacketId(PacketId.ChunkSync);

            // chunkCoords
            chunkSync.WriteBits(BitConverter.GetBytes(0u), 32 - Util.NumBitsRequiredUInt32(32), true); // X
            chunkSync.WriteBits(BitConverter.GetBytes(0u), 32 - Util.NumBitsRequiredUInt32(32), true); // Y
            chunkSync.WriteBits(BitConverter.GetBytes(0u), 32 - Util.NumBitsRequiredUInt32(32), true); // Z

            // hasData1
            chunkSync.Write1();

            var data1 = new byte[32 * 32 + 1];

            // fill with dirt
            for (var i = 1; i < data1.Length; i++)
                data1[i] = 24;

            chunkSync.Write(data1.Length);
            chunkSync.WriteAlignedBytes(data1, (uint)data1.Length);

            // hasData2
            chunkSync.Write0();

            // ?
            chunkSync.Write0();

            connection.Send(chunkSync);
        }

        {
            var chunkSync = new BitStream();

            chunkSync.WritePacketId(PacketId.ChunkSync);

            // chunkCoords
            chunkSync.WriteBits(BitConverter.GetBytes(1u), 32 - Util.NumBitsRequiredUInt32(32), true); // X
            chunkSync.WriteBits(BitConverter.GetBytes(0u), 32 - Util.NumBitsRequiredUInt32(32), true); // Y
            chunkSync.WriteBits(BitConverter.GetBytes(0u), 32 - Util.NumBitsRequiredUInt32(32), true); // Z

            // hasData1
            chunkSync.Write1();

            var data1 = new byte[32 * 32 * 10 + 1];

            // empty chunk
            for (var i = 0; i < 32 * 32 * 10; i++)
                data1[i + 1] = byte.MaxValue;

            // floor
            for (var i = 0; i < 32 * 32; i++)
                data1[i + 1] = 24;

            // tree - trunk
            data1[200 + 1024 + 1] = 13;
            data1[200 + 1024 + 1024 + 1] = 13;
            data1[200 + 1024 + 1024 + 1024 + 1] = 13;
            data1[200 + 1024 + 1024 + 1024 + 1024 + 1] = 13;

            // tree - leaves
            data1[135 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[136 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[137 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[166 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[167 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[168 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[169 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[170 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[198 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[199 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[201 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[202 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[230 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[231 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[232 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[233 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[234 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[263 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[264 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[265 + 1024 + 1024 + 1024 + 1024 + 1] = 14;

            // tree - leaves
            data1[135 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[136 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[137 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[166 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[167 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[168 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[169 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[170 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[198 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[199 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[201 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[202 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[230 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[231 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[232 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[233 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[234 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[263 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[264 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[265 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;

            // tree - leaves
            data1[167 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[168 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[169 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[199 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[201 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[231 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[232 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[233 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;

            // tree - leaves
            data1[168 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[199 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[201 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            data1[232 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;

            chunkSync.Write(data1.Length);
            chunkSync.WriteAlignedBytes(data1, (uint)data1.Length);

            // hasData2
            chunkSync.Write0();

            // ?
            chunkSync.Write0();

            connection.Send(chunkSync);
        }

        {
            var chunkSync = new BitStream();

            chunkSync.WritePacketId(PacketId.ChunkSync);

            // chunkCoords
            chunkSync.WriteBits(BitConverter.GetBytes(1u), 32 - Util.NumBitsRequiredUInt32(32), true); // X
            chunkSync.WriteBits(BitConverter.GetBytes(0u), 32 - Util.NumBitsRequiredUInt32(32), true); // Y
            chunkSync.WriteBits(BitConverter.GetBytes(1u), 32 - Util.NumBitsRequiredUInt32(32), true); // Z

            // hasData1
            chunkSync.Write1();

            var data1 = new byte[32 * 32 + 1];

            // fill with dirt
            for (var i = 1; i < data1.Length; i++)
                data1[i] = 24;

            chunkSync.Write(data1.Length);
            chunkSync.WriteAlignedBytes(data1, (uint)data1.Length);

            // hasData2
            chunkSync.Write0();

            // ?
            chunkSync.Write0();

            connection.Send(chunkSync);
        }

        {
            var chunkSync = new BitStream();

            chunkSync.WritePacketId(PacketId.ChunkSync);

            // chunkCoords
            chunkSync.WriteBits(BitConverter.GetBytes(0u), 32 - Util.NumBitsRequiredUInt32(32), true); // X
            chunkSync.WriteBits(BitConverter.GetBytes(0u), 32 - Util.NumBitsRequiredUInt32(32), true); // Y
            chunkSync.WriteBits(BitConverter.GetBytes(1u), 32 - Util.NumBitsRequiredUInt32(32), true); // Z

            // hasData1
            chunkSync.Write1();

            var data1 = new byte[32 * 32 + 1];

            // fill with dirt
            for (var i = 1; i < data1.Length; i++)
                data1[i] = 24;

            chunkSync.Write(data1.Length);
            chunkSync.WriteAlignedBytes(data1, (uint)data1.Length);

            // hasData2
            chunkSync.Write0();

            // ?
            chunkSync.Write0();

            connection.Send(chunkSync);
        }

        return true;
    }
}