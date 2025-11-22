using RakNet;
using SkySaga.Game.Extensions;
using System;
using System.Diagnostics;

namespace SkySaga.Game.Packets;

/// <summary>
/// PerformVoxelActions packet from client to server
/// Decodes voxel placement/destruction actions with bit-packed coordinates
///
/// Packet Structure (Bit-Packed, 128 bits / 16 bytes):
/// Offset  0- 15: Header? (16 bits)
/// Offset  5- 9:  chunkX (5 bits, range 0-31)
/// Offset  6:     Unknown gap (1 bit)
/// Offset 11- 15: chunkY (5 bits, range 0-31)
/// Offset 17- 21: chunkZ (5 bits, range 0-31)
/// Offset 23- 27: voxelX (5 bits, range 0-31)
/// Offset 28:     Unknown gap (1 bit)
/// Offset 29- 33: voxelY (5 bits, range 0-31)
/// Offset 34:     Unknown gap (1 bit)
/// Offset 35- 39: voxelZ (5 bits, range 0-31)
/// Offset 40- 42: side (3 bits, range 0-5, cube face)
/// Offset 43:     Unknown gap (1 bit)
/// Offset 44- 48: power (5 bits, stored as value/32 for fixed-point fraction)
/// NOTE: Direction and Position data are NOT decoded
/// </summary>
public static class PerformVoxelActions
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        // PerformVoxelActions packet is exactly 16 bytes (128 bits)
        const int packetBytes = 16;
        var allBytes = new byte[packetBytes];

        // Read exactly 16 bytes from the bitstream
        if (!bitStream.ReadAlignedBytes(allBytes, packetBytes))
            return false;

        // Extract chunk coordinates (offset 5, 11, 17 with 5 bits each)
        int chunkX = allBytes.ExtractBits(5, 5);
        int chunkY = allBytes.ExtractBits(11, 5);
        int chunkZ = allBytes.ExtractBits(17, 5);

        // Extract voxel coordinates (offset 23, 29, 35 with 5 bits each)
        int voxelX = allBytes.ExtractBits(23, 5);
        int voxelY = allBytes.ExtractBits(29, 5);
        int voxelZ = allBytes.ExtractBits(35, 5);

        // Extract side/face (offset 40 with 3 bits)
        int side = allBytes.ExtractBits(40, 3);

        // Extract power (offset 44 with 5 bits, stored as value/32)
        int powerBits = allBytes.ExtractBits(44, 5);
        float power = powerBits / 32f;

        Debug.WriteLine($"chunkCoords: ({chunkX}, {chunkY}, {chunkZ}), voxelCoords: ({voxelX}, {voxelY}, {voxelZ}), side: {side}, power: {power}", nameof(PerformVoxelActions));

        return true;
    }
}
