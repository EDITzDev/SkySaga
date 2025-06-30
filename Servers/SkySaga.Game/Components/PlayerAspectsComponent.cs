using System;

using RakNet;

using SkySaga.Game.Extensions;

namespace SkySaga.Game.Components;

public class PlayerAspectsComponent : Component
{
    public bool CanEditMap { get; set { field = value; OnParameterChanged(); } }
    public bool CanDamageEntities { get; set { field = value; OnParameterChanged(); } }
    public bool CanDamagePlayers { get; set { field = value; OnParameterChanged(); } }
    public bool CanCreateDevices { get; set { field = value; OnParameterChanged(); } }
    public bool CanDamageDevices { get; set { field = value; OnParameterChanged(); } }
    public bool IsSpectator { get; set { field = value; OnParameterChanged(); } }
    public bool IsTeleporting { get; set { field = value; OnParameterChanged(); } }
    public bool IsDebugPlayer { get; set { field = value; OnParameterChanged(); } }
    public string? Tags { get; set { field = value; OnParameterChanged(); } }
    public int AccountLevel { get; set { field = value; OnParameterChanged(); } }

    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        if (parameterName.Equals(nameof(CanEditMap), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(CanEditMap);

            return true;
        }
        else if (parameterName.Equals(nameof(CanDamageEntities), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(CanDamageEntities);

            return true;
        }
        else if (parameterName.Equals(nameof(CanDamagePlayers), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(CanDamagePlayers);

            return true;
        }
        else if (parameterName.Equals(nameof(CanCreateDevices), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(CanCreateDevices);

            return true;
        }
        else if (parameterName.Equals(nameof(CanDamageDevices), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(CanDamageDevices);

            return true;
        }
        else if (parameterName.Equals(nameof(IsSpectator), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(IsSpectator);

            return true;
        }
        else if (parameterName.Equals(nameof(IsTeleporting), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(IsTeleporting);

            return true;
        }
        else if (parameterName.Equals(nameof(IsDebugPlayer), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.Write(IsDebugPlayer);

            return true;
        }
        else if (parameterName.Equals(nameof(Tags), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteString(Tags);

            return true;
        }
        else if (parameterName.Equals(nameof(AccountLevel), StringComparison.OrdinalIgnoreCase))
        {
            bitStream.WriteBits(BitConverter.GetBytes(AccountLevel), 32 - Util.NumBitsRequiredUInt32(3), true);

            return true;
        }

        return false;
    }
}