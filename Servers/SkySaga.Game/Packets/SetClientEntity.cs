using RakNet;

using SkySaga.Game.Extensions;
using SkySaga.Game.Interfaces;

namespace SkySaga.Game.Packets;

public class SetClientEntity : ISerializablePacket
{
    public int EntityId;

    public BitStream Serialize()
    {
        var bitStream = new BitStream();

        bitStream.WritePacketId(PacketId.SetClientEntity);

        bitStream.Write(EntityId);

        return bitStream;
    }
}