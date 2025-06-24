using System;
using System.Collections.Generic;

using RakNet;

namespace SkySaga.Game.Components;

public class ClientFeatureUnlockComponent : Component
{
    public List<bool> FeatureIsLockedStatusList { get; set { field = value; OnParameterChanged(); } } = [];

    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        if (parameterName.Equals(nameof(FeatureIsLockedStatusList), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write0();

            for (var i = 0; i < 30; i++)
                bitStream.Write0();

            return true;
        }

        return false;
    }
}