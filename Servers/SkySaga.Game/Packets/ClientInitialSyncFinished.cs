using RakNet;

using System;
using System.Diagnostics;

namespace SkySaga.Game.Packets;

internal static class ClientInitialSyncFinished
{
    private static int _entityId = 1;

    public static bool Handle(Connection connection, BitStream bitStream)
    {
        Debug.WriteLine("", nameof(ClientInitialSyncFinished));

        connection.EntityId = _entityId++;

        {
            var entityAdd = new BitStream();

            entityAdd.WritePacketId(PacketId.EntityAdd);

            // Entity Name Hash
            entityAdd.Write1();
            entityAdd.Write((int)Util.ComputeCrc32("Player"));

            entityAdd.Write(connection.EntityId); // Id

            // ?
            entityAdd.Write0();

            var parameterData = new BitStream();

            // craftingdropslots
            parameterData.Write1();
            parameterData.Write(2);

            parameterData.Write(0);
            parameterData.Write(0);

            // featureislockedstatuslist
            parameterData.Write0();

            for (var i = 0; i < 30; i++)
                parameterData.Write0();

            parameterData.WriteBits(BitConverter.GetBytes(100), 32 - Util.NumBitsRequiredUInt32(0x200u), true); // halfhearts

            // position
            parameterData.WriteBits(BitConverter.GetBytes(2269), 32 - Util.NumBitsRequiredUInt32(0x10000u), true); // X
            parameterData.WriteBits(BitConverter.GetBytes(70), 32 - Util.NumBitsRequiredUInt32(0x10000u), true); // Y
            parameterData.WriteBits(BitConverter.GetBytes(629), 32 - Util.NumBitsRequiredUInt32(0x10000u), true); // Z

            var syncData = new BitStream();

            syncData.WriteParameterFlags(89, 17, 27, 29, 66); // craftingdropslots, featureislockedstatuslist, halfhearts, position

            syncData.WriteBits(BitConverter.GetBytes(parameterData.GetNumberOfBitsUsed()), 32 - Util.NumBitsRequiredUInt32(0x20000));

            if (parameterData.GetNumberOfBitsUsed() > 0)
                syncData.Write(parameterData);

            entityAdd.WriteBits(BitConverter.GetBytes(syncData.GetNumberOfBitsUsed()), 32 - Util.NumBitsRequiredUInt32(0x20000));

            if (syncData.GetNumberOfBitsUsed() > 0)
                entityAdd.Write(syncData);

            connection.Send(entityAdd);
        }

        {
            var entityAdd = new BitStream();

            entityAdd.WritePacketId(PacketId.EntityAdd);

            // Entity Name Hash
            entityAdd.Write1();
            entityAdd.Write((int)Util.ComputeCrc32("Sheep"));

            entityAdd.Write(_entityId++); // Id

            // ?
            entityAdd.Write0();

            var parameterData = new BitStream();

            // position
            parameterData.WriteBits(BitConverter.GetBytes(1000), 32 - Util.NumBitsRequiredUInt32(0x10000u), true); // X
            parameterData.WriteBits(BitConverter.GetBytes(63), 32 - Util.NumBitsRequiredUInt32(0x10000u), true); // Y
            parameterData.WriteBits(BitConverter.GetBytes(1000), 32 - Util.NumBitsRequiredUInt32(0x10000u), true); // Z

            var syncData = new BitStream();

            syncData.WriteParameterFlags(35, 23); // position

            syncData.WriteBits(BitConverter.GetBytes(parameterData.GetNumberOfBitsUsed()), 32 - Util.NumBitsRequiredUInt32(0x20000));

            if (parameterData.GetNumberOfBitsUsed() > 0)
                syncData.Write(parameterData);

            entityAdd.WriteBits(BitConverter.GetBytes(syncData.GetNumberOfBitsUsed()), 32 - Util.NumBitsRequiredUInt32(0x20000));

            if (syncData.GetNumberOfBitsUsed() > 0)
                entityAdd.Write(syncData);

            connection.Send(entityAdd);
        }

        var clientEntitiesSyncFinished = new BitStream();

        clientEntitiesSyncFinished.WritePacketId(PacketId.ClientEntitiesSyncFinished);

        connection.Send(clientEntitiesSyncFinished);

        return true;
    }
}