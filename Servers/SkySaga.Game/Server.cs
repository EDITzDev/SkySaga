using System;
using System.Collections.Generic;

using RakNet;

namespace SkySaga.Game;

internal class Server : IDisposable
{
    private const int MaxConnections = 100;

    private readonly ushort _port;
    private readonly RakPeerInterface _peer;

    private readonly Dictionary<ulong, Connection> _connections = new();

    public Server(string password, ushort port)
    {
        _port = port;
        _peer = RakPeerInterface.GetInstance();

#if DEBUG
        var packetLogger = new PacketLogger();
        _peer.AttachPlugin(packetLogger);
#endif

        _peer.SetIncomingPassword(password, password.Length);
        _peer.SetMaximumIncomingConnections(MaxConnections);
    }

    public bool Start()
    {
        var status = _peer.Startup(MaxConnections, new SocketDescriptor(_port, null), 1);

        return status == StartupResult.RAKNET_STARTED;
    }

    public void Tick()
    {
        var packet = _peer.Receive();

        if (packet is null)
            return;

        if (!_connections.TryGetValue(packet.guid.g, out var connection))
        {
            connection = new Connection(packet.guid, this);

            _connections.Add(packet.guid.g, connection);
        }

        connection.ProcessPacket(packet);

        _peer.DeallocatePacket(packet);
    }

    public void Send(BitStream bitStream, AddressOrGUID systemIdentifier)
    {
        _peer.Send(bitStream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, systemIdentifier, false);
    }

    public void Dispose()
    {
        RakPeerInterface.DestroyInstance(_peer);
    }
}