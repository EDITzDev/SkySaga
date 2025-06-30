using System;

using RakNet;

using SkySaga.Game.Packets.Common;

namespace SkySaga.Game.Components;

public class WalletComponent : Component
{
    public WalletData Currency { get; set { field = value; OnParameterChanged(); } } = new();

    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        if (parameterName.Equals(nameof(Currency), StringComparison.OrdinalIgnoreCase))
        {
            Currency.Serialize(bitStream);

            return true;
        }

        return false;
    }
}