using System;

using RakNet;

namespace SkySaga.Game.Components;

public class ClientHealthComponent : HealthComponent
{
    public byte DebrisType { get; set { field = value; OnParameterChanged(); } }

    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        if (base.TrySync(parameterName, bitStream))
            return true;

        if (parameterName.Equals(nameof(DebrisType), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(DebrisType);

            return true;
        }

        return false;
    }
}