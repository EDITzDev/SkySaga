using System.Diagnostics;
using System.Collections.Generic;

using RakNet;

namespace SkySaga.Game.Packets;

public static class ExecuteEntityAction
{
    private static Dictionary<uint, string> _actions = new()
    {
        { Util.ComputeCrc32("PlaceVoxelAction"), "PlaceVoxelAction" },
        { Util.ComputeCrc32("DigAction"), "DigAction" },
        { Util.ComputeCrc32("ActivatePortalAction"), "ActivatePortalAction" },
        { Util.ComputeCrc32("ThrowPickupAction"), "ThrowPickupAction" },
        { Util.ComputeCrc32("AttackAction"), "AttackAction" },
        { Util.ComputeCrc32("CreateDeviceAction"), "CreateDeviceAction" },
        { Util.ComputeCrc32("CreateEntityAction"), "CreateEntityAction" },
        { Util.ComputeCrc32("CreatePickupEntityAction"), "CreatePickupEntityAction" },
        { Util.ComputeCrc32("PickupAction"), "PickupAction" },
        { Util.ComputeCrc32("InteractAction"), "InteractAction" },
        { Util.ComputeCrc32("LaunchProjectileAction"), "LaunchProjectileAction" },
        { Util.ComputeCrc32("EatAction"), "EatAction" },
        { Util.ComputeCrc32("LearnEmoteAction"), "LearnEmoteAction" },
        { Util.ComputeCrc32("LearnRecipeAction"), "LearnRecipeAction" },
        { Util.ComputeCrc32("LearnHomeIslandTitleAction"), "LearnHomeIslandTitleAction" },
        { Util.ComputeCrc32("UnlockJobChallengeAction"), "UnlockJobChallengeAction" },
        { Util.ComputeCrc32("BlockAction"), "BlockAction" },
        { Util.ComputeCrc32("BlastRadiusAction"), "BlastRadiusAction" },
        { Util.ComputeCrc32("ResourcePickupAction"), "ResourcePickupAction" },
    };

    public static bool Handle(Connection connection, BitStream bitStream)
    {
        if (!bitStream.Read(out int srcEntityID))
            return false;

        if (!bitStream.Read(out int targetEntityID))
            return false;

        var actionCrc = 0;
        var hasActionCrc = bitStream.ReadBit();

        if (hasActionCrc && !bitStream.Read(out actionCrc))
            return false;

        _actions.TryGetValue((uint)actionCrc, out var actionName);

        Debug.WriteLine($"srcEntityID: {srcEntityID}, targetEntityID: {targetEntityID}, actionCrc: {actionCrc}, actionName: {actionName}", nameof(ExecuteEntityAction));

        return true;
    }
}