using RakNet;
using SkySaga.Game.Enums;
using SkySaga.Game.Extensions;
using System.Diagnostics;

namespace SkySaga.Game.Packets;

/// <summary>
/// PerformVoxelActions packet from client to server
/// Decodes voxel placement/destruction actions with bit-packed coordinates
///
/// Packet Structure (Bit-Packed):
/// Offset   0- 3:   location (4 bits, range 0-15)
/// Offset   4- 9:   chunkCoordX (6 bits, range 0-63)
/// Offset  10-15:   chunkCoordY (6 bits, range 0-63)
/// Offset  16-21:   chunkCoordZ (6 bits, range 0-63)
/// Offset  22-27:   voxelCoordX (6 bits, range 0-63)
/// Offset  28-33:   voxelCoordY (6 bits, range 0-63)
/// Offset  34-39:   voxelCoordZ (6 bits, range 0-63)
/// Offset  40-42:   side (3 bits, range 0-5, cube face)
/// Offset  43-48:   power (6 bits, range 0-63)
/// Offset  49-65:   positionX (17 bits)
/// Offset  66-82:   positionY (17 bits)
/// Offset  83-99:   positionZ (17 bits)
/// Offset 100-107:  directionX (8 bits)
/// Offset 108-115:  directionY (8 bits)
/// Offset 116-123:  directionZ (8 bits)
/// </summary>
public static class PerformVoxelActions
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        // Location
        if (!ReadLocation(bitStream, out var location)) return false;

        // ChunkCoords
        if (!ReadChunkCoordinate(bitStream, out var chunkCoordX)) return false;
        if (!ReadChunkCoordinate(bitStream, out var chunkCoordY)) return false;
        if (!ReadChunkCoordinate(bitStream, out var chunkCoordZ)) return false;

        // VoxelCoords
        if (!ReadVoxelCoordinate(bitStream, out var voxelCoordX)) return false;
        if (!ReadVoxelCoordinate(bitStream, out var voxelCoordY)) return false;
        if (!ReadVoxelCoordinate(bitStream, out var voxelCoordZ)) return false;

        // Side
        if (!ReadSide(bitStream, out var side)) return false;

        // Power
        if (!ReadPower(bitStream, out var power)) return false;

        // Position
        if (!ReadPosition(bitStream, out var positionX)) return false;
        if (!ReadPosition(bitStream, out var positionY)) return false;
        if (!ReadPosition(bitStream, out var positionZ)) return false;

        // Direction
        if (!ReadDirection(bitStream, out var directionX)) return false;
        if (!ReadDirection(bitStream, out var directionY)) return false;
        if (!ReadDirection(bitStream, out var directionZ)) return false;

        Debug.WriteLine($"location: {location}, chunkCoord: ({chunkCoordX}, {chunkCoordY}, {chunkCoordZ}), voxelCoord: ({voxelCoordX}, {voxelCoordY}, {voxelCoordZ}), side: {side}, power: {power}, position: ({positionX}, {positionY}, {positionZ}), direction: ({directionX}, {directionY}, {directionZ})", nameof(PerformVoxelActions));

        return true;
    }

    private static bool ReadLocation(BitStream bitStream, out BlockLocation location)
    {
        if (!bitStream.ReadByte(8, out var tmpLocation))
        {
            location = 0;
            return false;
        }
        location = (BlockLocation)tmpLocation;
        return true;
    }

    private static bool ReadChunkCoordinate(BitStream bitStream, out int coord)
        => bitStream.ReadInt32(32, out coord);

    private static bool ReadVoxelCoordinate(BitStream bitStream, out int coord)
        => bitStream.ReadInt32(32, out coord);

    private static bool ReadSide(BitStream bitStream, out BlockSide side)
    {
        if (!bitStream.ReadByte(6, out var tmpSide))
        {
            side = 0;
            return false;
        }
        side = (BlockSide)tmpSide;
        return true;
    }

    private static bool ReadPower(BitStream bitStream, out float power)
    {
        if (!bitStream.ReadInt32(32, out var tmpPower))
        {
            power = 0;
            return false;
        }
        power = tmpPower / 32f;
        return true;
    }

    private static bool ReadPosition(BitStream bitStream, out float position)
    {
        if (!bitStream.ReadInt32(0x10000, out var tmpPosition))
        {
            position = 0;
            return false;
        }
        position = tmpPosition / 64f;
        return true;
    }

    private static bool ReadDirection(BitStream bitStream, out int direction)
    {
        if (!bitStream.ReadInt32(128, out var tmpDirection))
        {
            direction = 0;
            return false;
        }
        direction = tmpDirection / 64 - 1;
        return true;
    }
}
