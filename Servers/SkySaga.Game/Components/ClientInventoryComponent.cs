using System;

using RakNet;

namespace SkySaga.Game.Components;

public class ClientInventoryComponent : InventoryComponent
{
    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        return base.TrySync(parameterName, bitStream);
    }
}