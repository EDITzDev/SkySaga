using System;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Frozen;
using System.Collections.Generic;

using RakNet;

using SkySaga.Game.World;
using SkySaga.Game.Packets;
using SkySaga.Game.Extensions;
using SkySaga.Game.Interfaces;
using SkySaga.Game.Components;

namespace SkySaga.Game;

public class Server : IDisposable
{
    private const int MaxConnections = 100;

    private readonly ushort _port;
    private readonly RakPeerInterface _peer;

    private readonly Dictionary<int, Map> _maps = new();
    private readonly Dictionary<ulong, Connection> _connections = new();

    public Server(string password, ushort port)
    {
        _port = port;
        _peer = RakPeerInterface.GetInstance();

        /* Causing Crash !?

        #if DEBUG
                var packetLogger = new PacketLogger();
                _peer.AttachPlugin(packetLogger);
        #endif

        */

        _peer.SetIncomingPassword(password, password.Length);
        _peer.SetMaximumIncomingConnections(MaxConnections);

        // TODO: Map system that'll load entities and their states

        var map = new Map(new MapDefinition
        {
            MapSizeChunks = new Vector<int>(4),
            BiomeType = Util.ComputeCrc32("Sky_Island"),
            GameMode = 1
        });

        if (map.TryCreateEntity("Sheep", out var sheep))
        {
            if (sheep.TryGetComponent("SmoothedTransformComponent", out var component) &&
                component is SmoothedTransformComponent smoothedTransformComponent)
            {
                smoothedTransformComponent.Position = new Vector<int>([2269, 70, 629, 0, 0, 0, 0, 0]);
            }

            if (sheep.TryGetComponent("ClientHealthComponent", out component) &&
                component is ClientHealthComponent transformComponent)
            {
                transformComponent.HalfHearts = 50;
            }
        }

        _maps.TryAdd(0, map);
    }

    public bool Start()
    {
        var status = _peer.Startup(MaxConnections, new SocketDescriptor(_port, string.Empty), 1);

        return status == StartupResult.RAKNET_STARTED;
    }

    public void Tick()
    {
        ProcessPackets();

        ProcessMaps();
        ProcessConnections();
    }

    private void ProcessPackets()
    {
        var packet = _peer.Receive();

        if (packet is null)
            return;

        var bitStream = new BitStream(packet.data, packet.length, false);

        var messageId = bitStream.ReadMessageId();

        if (!_connections.TryGetValue(packet.guid.g, out var connection)
            && messageId == (byte)DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION)
        {
            // TODO: Decide which map the connection uses
            connection = new Connection(this, _maps[0], packet.guid);

            _connections.TryAdd(packet.guid.g, connection);

            OnConnectionAdded(connection);

            goto Deallocate;
        }

        if (connection is null)
            goto Deallocate;

        if (messageId == (byte)DefaultMessageIDTypes.ID_CONNECTION_LOST ||
            messageId == (byte)DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION)
        {
            if (!_connections.Remove(packet.guid.g, out connection))
                throw new InvalidOperationException();

            OnConnectionRemoved(connection);

            goto Deallocate;
        }

        if (messageId >= (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM)
        {
            var packetId = (PacketId)messageId - (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM;

            var handled = connection.ProcessPacket(packetId, bitStream);

            if (!handled)
                Debug.WriteLine($"Unhandled Packet. ( Length: {packet.length} )", packetId.ToString());
        }

    Deallocate:
        _peer.DeallocatePacket(packet);
    }

    private void ProcessMaps()
    {
        foreach (var map in _maps.ToFrozenSet())
        {
            var entities = map.Value.Entities;

            foreach (var entity in entities)
            {
                if (!entity.SyncRequired)
                    continue;

                var entitySync = new EntitySync
                {
                    Id = entity.Id,
                    SyncData = entity.GetSyncData(newEntity: false)
                };

                SendToAll(entitySync);
            }
        }
    }

    private void ProcessConnections()
    {
        foreach (var connection in _connections.ToFrozenSet())
        {
            connection.Value.Tick();
        }
    }

    public void Send(BitStream bitStream, AddressOrGUID systemIdentifier)
    {
        _peer.Send(bitStream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, systemIdentifier, false);
    }

    public void SendToAll(ISerializablePacket packet)
    {
        var bitStream = packet.Serialize();

        foreach (var connection in _connections.ToFrozenDictionary())
            connection.Value.Send(bitStream);
    }

    private void OnConnectionAdded(Connection connection)
    {
        connection.OnConnected();
    }

    private void OnConnectionRemoved(Connection connection)
    {
        _connections.Remove(connection.Guid.g);

        connection.OnDisconnected();
    }

    public void Dispose()
    {
        RakPeerInterface.DestroyInstance(_peer);
    }
}