using System;

using RakNet;

using SkySaga.Game.Extensions;

namespace SkySaga.Game.Components;

public class TimeOfDayComponent : Component
{
    public int StartTimeOfDay { get; set { field = value; OnParameterChanged(); } }
    public bool FixedTimeOfDay { get; set { field = value; OnParameterChanged(); } }
    public int DayNightCycleDuration { get; set { field = value; OnParameterChanged(); } }
    public ulong RealWorldStartTime { get; set { field = value; OnParameterChanged(); } }
    public int TimeStretch { get; set { field = value; OnParameterChanged(); } }
    public int TimeOfDayOffset { get; set { field = value; OnParameterChanged(); } }

    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        if (parameterName.Equals(nameof(StartTimeOfDay), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteBits(BitConverter.GetBytes(StartTimeOfDay), 32 - Util.NumBitsRequiredUInt32(0x10000), true);

            return true;
        }
        else if (parameterName.Equals(nameof(FixedTimeOfDay), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(FixedTimeOfDay);

            return true;
        }
        else if (parameterName.Equals(nameof(DayNightCycleDuration), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteBits(BitConverter.GetBytes(DayNightCycleDuration), 32 - Util.NumBitsRequiredUInt32(1920), true);

            return true;
        }
        else if (parameterName.Equals(nameof(RealWorldStartTime), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteUInt64(RealWorldStartTime);

            return true;
        }
        else if (parameterName.Equals(nameof(TimeStretch), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteBits(BitConverter.GetBytes(TimeStretch), 32 - Util.NumBitsRequiredUInt32(8128), true);

            return true;
        }
        else if (parameterName.Equals(nameof(TimeOfDayOffset), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteBits(BitConverter.GetBytes(TimeOfDayOffset), 32 - Util.NumBitsRequiredUInt32(0x10000), true);

            return true;
        }

        return false;
    }
}