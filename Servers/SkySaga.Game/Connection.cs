using System.Diagnostics;

using RakNet;

using SkySaga.Game.Packets;

namespace SkySaga.Game;

internal class Connection
{
    private readonly Server _server;
    private readonly RakNetGUID _guid;

    public int EntityId { get; set; }

    public Connection(RakNetGUID guid, Server server)
    {
        _guid = guid;
        _server = server;
    }

    public void ProcessPacket(Packet packet)
    {
        var bitStream = new BitStream(packet.data, packet.length, false);

        var messageId = bitStream.ReadMessageId();

        if (messageId < (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM)
            return;

        var packetId = (PacketId)messageId - (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM;

        var handled = packetId switch
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

        if (!handled)
            Debug.WriteLine($"Unhandled Packet. ( Length: {packet.length} )", packetId.ToString());
    }

    public void Send(BitStream bitStream)
    {
        _server.Send(bitStream, _guid);
    }
}