using System;

using RakNet;

using SkySaga.Game.Packets.Common;

namespace SkySaga.Game.Components;

public class InventoryItemComponent : Component
{
    public InventorySlotData InventorySlotData { get; set { field = value; OnParameterChanged(); } } = new();
    public bool ItemLocked { get; set { field = value; OnParameterChanged(); } }
    public bool AllowAddingToFoundInBiomes { get; set { field = value; OnParameterChanged(); } }
    public bool HasBeenTransferred { get; set { field = value; OnParameterChanged(); } }

    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        if (parameterName.Equals(nameof(InventorySlotData), StringComparison.OrdinalIgnoreCase))
        {
            InventorySlotData.Serialize(bitStream);

            return true;
        }
        else if (parameterName.Equals(nameof(ItemLocked), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(ItemLocked);

            return true;
        }
        else if (parameterName.Equals(nameof(AllowAddingToFoundInBiomes), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(AllowAddingToFoundInBiomes);

            return true;
        }
        else if (parameterName.Equals(nameof(HasBeenTransferred), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(HasBeenTransferred);

            return true;
        }

        return false;
    }
}