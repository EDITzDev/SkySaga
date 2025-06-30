using RakNet;

namespace SkySaga.Game.Components;

public class ClientTimeOfDayComponent : TimeOfDayComponent
{
    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        return base.TrySync(parameterName, bitStream);
    }
}