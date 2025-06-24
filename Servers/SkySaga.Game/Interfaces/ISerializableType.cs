using RakNet;

namespace SkySaga.Game.Interfaces;

public interface ISerializableType
{
    void Serialize(BitStream bitStream);
}