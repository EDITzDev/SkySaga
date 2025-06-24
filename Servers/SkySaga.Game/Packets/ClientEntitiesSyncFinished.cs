using RakNet;

using SkySaga.Game.Extensions;
using SkySaga.Game.Interfaces;

namespace SkySaga.Game.Packets;

public class ClientEntitiesSyncFinished : ISerializablePacket
{
    public BitStream Serialize()
    {
        var bitStream = new BitStream();

        bitStream.WritePacketId(PacketId.ClientEntitiesSyncFinished);

        return bitStream;
    }
}