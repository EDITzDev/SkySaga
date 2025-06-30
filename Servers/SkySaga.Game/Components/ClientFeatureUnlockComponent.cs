using RakNet;

namespace SkySaga.Game.Components;

public class ClientFeatureUnlockComponent : FeatureUnlockComponent
{

    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        return base.TrySync(parameterName, bitStream);
    }
}