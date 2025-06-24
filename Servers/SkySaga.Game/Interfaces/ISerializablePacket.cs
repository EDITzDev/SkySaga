using RakNet;

namespace SkySaga.Game.Interfaces;

public interface ISerializablePacket
{
    BitStream Serialize();
}