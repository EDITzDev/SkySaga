using System;

using RakNet;

using SkySaga.Game.Packets.Common;

namespace SkySaga.Game.Components;

public class HealthComponent : Component
{
    public int WholeHearts { get; set { field = value; OnParameterChanged(); } }
    public int HalfHearts { get; set { field = value; OnParameterChanged(); } }
    public int InitialHP { get; set { field = value; OnParameterChanged(); } }
    public bool Immortal { get; set { field = value; OnParameterChanged(); } }
    public int CorpseStatus { get; set { field = value; OnParameterChanged(); } }
    public int LastDamageSourceID { get; set { field = value; OnParameterChanged(); } }
    public ItemSpec LastDamageSourceWeaponItemSpec { get; set { field = value; OnParameterChanged(); } } = new();

    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        if (parameterName.Equals(nameof(WholeHearts), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteBits(BitConverter.GetBytes(WholeHearts), 32 - Util.NumBitsRequiredUInt32(0x200), true);

            return true;
        }
        else if (parameterName.Equals(nameof(HalfHearts), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteBits(BitConverter.GetBytes(HalfHearts), 32 - Util.NumBitsRequiredUInt32(0x200), true);

            return true;
        }
        else if (parameterName.Equals(nameof(InitialHP), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteBits(BitConverter.GetBytes(InitialHP), 32 - Util.NumBitsRequiredUInt32(0x200), true);

            return true;
        }
        else if (parameterName.Equals(nameof(Immortal), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(Immortal);

            return true;
        }
        else if (parameterName.Equals(nameof(CorpseStatus), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteBits(BitConverter.GetBytes(CorpseStatus), 32 - Util.NumBitsRequiredUInt32(4), true);

            return true;
        }
        else if (parameterName.Equals(nameof(LastDamageSourceID), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(LastDamageSourceID);

            return true;
        }
        else if (parameterName.Equals(nameof(LastDamageSourceWeaponItemSpec), StringComparison.OrdinalIgnoreCase))
        {
            LastDamageSourceWeaponItemSpec.Serialize(bitStream);

            return true;
        }

        return false;
    }
}