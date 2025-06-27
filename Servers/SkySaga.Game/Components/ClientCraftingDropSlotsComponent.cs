using System;

using RakNet;

namespace SkySaga.Game.Components;

public class ClientCraftingDropSlotsComponent : CraftingDropSlotsComponent
{
    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        return base.TrySync(parameterName, bitStream);
    }
}