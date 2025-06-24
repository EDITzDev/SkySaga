using RakNet;

using SkySaga.Game.Extensions;
using SkySaga.Game.Interfaces;

namespace SkySaga.Game.Packets;

public class ServerInfo : ISerializablePacket
{
    public string? ServerOwnerGuid;
    public string? ServerOwnerName;

    public string? ServerBiome;

    public uint? ServerAdventureCrc;

    public int MapHeaderSeed;

    public bool IsHomeWorld;
    public bool IsMyWorld;

    public string? ChatHost;
    public ushort ChatPort;

    public BitStream Serialize()
    {
        var bitStream = new BitStream();

        bitStream.WritePacketId(PacketId.ServerInfo);

        bitStream.WriteString(ServerOwnerGuid);
        bitStream.WriteString(ServerOwnerName);

        bitStream.WriteString(ServerBiome);

        bitStream.WriteOptional(ServerAdventureCrc, (value) =>
        {
            bitStream.Write((int)value);
        });

        bitStream.Write(MapHeaderSeed);

        bitStream.Write(IsHomeWorld);
        bitStream.Write(IsMyWorld);

        bitStream.WriteString(ChatHost);
        bitStream.Write(ChatPort);

        return bitStream;
    }
}