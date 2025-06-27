using System;
using System.Collections.Generic;

using RakNet;

using SkySaga.Game.Extensions;

namespace SkySaga.Game.Components;

public class InventoryComponent : Component
{
    public byte MaxInventorySlots { get; set { field = value; OnParameterChanged(); } }
    public uint? InventoryLoadOut { get; set { field = value; OnParameterChanged(); } }
    public bool TakeOnly { get; set { field = value; OnParameterChanged(); } }

    private const int InventoryEntityListDefaultCount = 45;
    public List<int> InventoryEntityList { get; set { field = value; OnParameterChanged(); } } = [];

    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        if (parameterName.Equals(nameof(MaxInventorySlots), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteBits([MaxInventorySlots], 8 - Util.NumBitsRequiredByte(36), true);

            return true;
        }
        else if (parameterName.Equals(nameof(InventoryLoadOut), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteOptional(InventoryLoadOut, (value) =>
            {
                bitStream.Write((int)value);
            });

            return true;
        }
        else if (parameterName.Equals(nameof(TakeOnly), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(TakeOnly);

            return true;
        }
        else if (parameterName.Equals(nameof(InventoryEntityList), StringComparison.OrdinalIgnoreCase))
        {
            // Count is optimised
            if (InventoryEntityList.Count < InventoryEntityListDefaultCount)
            {
                bitStream.WriteBits(BitConverter.GetBytes(InventoryEntityList.Count), 32 - Util.NumBitsRequiredUInt32(InventoryEntityListDefaultCount), true);
            }
            else
            {
                bitStream.WriteBits(BitConverter.GetBytes(InventoryEntityListDefaultCount), 32 - Util.NumBitsRequiredUInt32(InventoryEntityListDefaultCount), true);

                bitStream.Write1();
                bitStream.Write(InventoryEntityList.Count);
            }

            foreach (var inventoryEntity in InventoryEntityList)
                bitStream.Write(inventoryEntity);

            return true;
        }

        return false;
    }
}