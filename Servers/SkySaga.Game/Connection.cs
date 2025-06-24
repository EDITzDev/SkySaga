using System;
using System.Numerics;

using RakNet;

using SkySaga.Game.World;
using SkySaga.Game.Packets;
using SkySaga.Game.Entities;
using SkySaga.Game.Components;
using SkySaga.Game.Interfaces;

namespace SkySaga.Game;

public class Connection
{
    private readonly Server _server;

    public readonly RakNetGUID Guid;

    public Map Map { get; }
    public Entity Player { get; }

    public Connection(Server server, Map map, RakNetGUID guid)
    {
        _server = server;

        Map = map;
        Guid = guid;

        Map.TryCreateEntity("Player", out var player);

        ArgumentNullException.ThrowIfNull(player);

        Player = player;
    }

    public void OnConnected()
    {
    }

    public void OnDisconnected()
    {
        Map.RemoveEntity(Player);

        var entityRemoved = new EntityRemoved
        {
            Id = Player.Id
        };

        _server.SendToAll(entityRemoved);
    }

    public bool ProcessPacket(PacketId packetId, BitStream bitStream)
    {
        return packetId switch
        {
            PacketId.ClientConnected => ClientConnected.Handle(this, bitStream),
            PacketId.ClientReadyToSync => ClientReadyToSync.Handle(this, bitStream),
            PacketId.ClientInitialSyncFinished => ClientInitialSyncFinished.Handle(this, bitStream),
            PacketId.ClientReadyToPlay => ClientReadyToPlay.Handle(this, bitStream),
            PacketId.SetLookAtDirection => SetLookAtDirection.Handle(this, bitStream),
            PacketId.EntityMoved => EntityMoved.Handle(this, bitStream),
            PacketId.ExecuteEntityAction => ExecuteEntityAction.Handle(this, bitStream),
            _ => false
        };
    }

    public void Tick()
    {
    }

    public void Send(BitStream bitStream)
    {
        _server.Send(bitStream, Guid);
    }

    public void Send(ISerializablePacket packet)
    {
        _server.Send(packet.Serialize(), Guid);
    }

    public void InitialChunkSync()
    {
        // TODO: Chucking System

        var beginSync = new BeginSync
        {
            NumChunksToSync = 4
        };

        Send(beginSync);

        {
            var chunkSync = new ChunkSync
            {
                Coords = new Vector<int>([0, 0, 0, 0, 0, 0, 0, 0])
            };

            chunkSync.Data1 = new byte[32 * 32 + 1];

            // fill with dirt
            for (var i = 1; i < chunkSync.Data1.Length; i++)
                chunkSync.Data1[i] = 24;

            Send(chunkSync);
        }

        {
            var chunkSync = new ChunkSync
            {
                Coords = new Vector<int>([1, 0, 0, 0, 0, 0, 0, 0])
            };

            chunkSync.Data1 = new byte[32 * 32 * 10 + 1];

            // empty chunk
            for (var i = 0; i < 32 * 32 * 10; i++)
                chunkSync.Data1[i + 1] = byte.MaxValue;

            // floor
            for (var i = 0; i < 32 * 32; i++)
                chunkSync.Data1[i + 1] = 24;

            // tree - trunk
            chunkSync.Data1[200 + 1024 + 1] = 13;
            chunkSync.Data1[200 + 1024 + 1024 + 1] = 13;
            chunkSync.Data1[200 + 1024 + 1024 + 1024 + 1] = 13;
            chunkSync.Data1[200 + 1024 + 1024 + 1024 + 1024 + 1] = 13;

            // tree - leaves
            chunkSync.Data1[135 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[136 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[137 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[166 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[167 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[168 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[169 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[170 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[198 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[199 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[201 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[202 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[230 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[231 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[232 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[233 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[234 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[263 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[264 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[265 + 1024 + 1024 + 1024 + 1024 + 1] = 14;

            // tree - leaves
            chunkSync.Data1[135 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[136 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[137 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[166 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[167 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[168 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[169 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[170 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[198 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[199 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[201 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[202 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[230 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[231 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[232 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[233 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[234 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[263 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[264 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[265 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;

            // tree - leaves
            chunkSync.Data1[167 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[168 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[169 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[199 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[201 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[231 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[232 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[233 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;

            // tree - leaves
            chunkSync.Data1[168 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[199 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[201 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;
            chunkSync.Data1[232 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1024 + 1] = 14;

            Send(chunkSync);
        }

        {
            var chunkSync = new ChunkSync
            {
                Coords = new Vector<int>([1, 0, 1, 0, 0, 0, 0, 0])
            };

            chunkSync.Data1 = new byte[32 * 32 + 1];

            // fill with dirt
            for (var i = 1; i < chunkSync.Data1.Length; i++)
                chunkSync.Data1[i] = 24;

            Send(chunkSync);
        }

        {
            var chunkSync = new ChunkSync
            {
                Coords = new Vector<int>([0, 0, 1, 0, 0, 0, 0, 0])
            };

            chunkSync.Data1 = new byte[32 * 32 + 1];

            // fill with dirt
            for (var i = 1; i < chunkSync.Data1.Length; i++)
                chunkSync.Data1[i] = 24;

            Send(chunkSync);
        }
    }

    public void InitialEntitiySync()
    {
        // TODO: Component property defaults and database

        if (Player.TryGetComponent("ClientHealthComponent", out var component) &&
            component is ClientHealthComponent transformComponent)
        {
            transformComponent.HalfHearts = 100;
        }

        if (Player.TryGetComponent("SmoothedTransformComponent", out component) &&
            component is SmoothedTransformComponent smoothedTransformComponent)
        {
            smoothedTransformComponent.Position = new Vector<int>([2269, 70, 629, 0, 0, 0, 0, 0]);
        }

        if (Player.TryGetComponent("ClientFeatureUnlockComponent", out component) &&
            component is ClientFeatureUnlockComponent clientFeatureUnlockComponent)
        {
            clientFeatureUnlockComponent.FeatureIsLockedStatusList.Add(true);
        }

        foreach (var entity in Map.Entities)
        {
            var entityAdd = new EntityAdd
            {
                Id = entity.Id,
                NameHash = Util.ComputeCrc32(entity.Name),
                SyncData = entity.GetSyncData(newEntity: true)
            };

            Send(entityAdd);
        }

        Send(new ClientEntitiesSyncFinished());
    }
}