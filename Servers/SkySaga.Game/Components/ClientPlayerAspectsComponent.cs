using RakNet;

namespace SkySaga.Game.Components;

public class ClientPlayerAspectsComponent : PlayerAspectsComponent
{
    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        return base.TrySync(parameterName, bitStream);
    }
}