using System;
using System.Diagnostics;

using RakNet;

namespace SkySaga.Game.Packets;

internal static class ClientConnected
{
    public static bool Handle(Connection connection, BitStream bitStream)
    {
        var clientVersionNumber = bitStream.ReadString();

        Debug.WriteLine($"{nameof(clientVersionNumber)}: {clientVersionNumber}", nameof(ClientConnected));

        var serverInfo = new BitStream();

        serverInfo.WritePacketId(PacketId.ServerInfo);

        serverInfo.WriteString("482f2571-e9a6-4f52-97bd-2231a87a9f9a"); // serverOwnerGUID
        serverInfo.WriteString("EDITz"); // serverOwnerName
        serverInfo.WriteString("Desert"); // serverBiome

        // serverAdventureCRC
        serverInfo.Write1();
        serverInfo.Write((int)Util.ComputeCrc32("FTUE_Adventure"));

        serverInfo.Write(0); // mapHeaderSeed

        serverInfo.Write0(); // isHomeWorld
        serverInfo.Write0(); // isMyWorld

        serverInfo.WriteString("127.0.0.1"); // chatHost
        serverInfo.Write((ushort)444); // chatPort

        connection.Send(serverInfo);

        var mapDefinition = new BitStream();

        mapDefinition.WritePacketId(PacketId.MapDefinition);

        // mapSizeChunks
        mapDefinition.WriteBits(BitConverter.GetBytes(4u), 32 - Util.NumBitsRequiredUInt32(32), true); // X
        mapDefinition.WriteBits(BitConverter.GetBytes(4u), 32 - Util.NumBitsRequiredUInt32(32), true); // Y
        mapDefinition.WriteBits(BitConverter.GetBytes(4u), 32 - Util.NumBitsRequiredUInt32(32), true); // Z

        // biomeType
        mapDefinition.Write0();

        mapDefinition.WriteBits(BitConverter.GetBytes(1u), 32 - Util.NumBitsRequiredUInt32(4), true); // gameMode

        connection.Send(mapDefinition);

        return true;
    }
}