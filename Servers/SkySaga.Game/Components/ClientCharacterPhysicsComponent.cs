using System;

using RakNet;

namespace SkySaga.Game.Components;

public class ClientCharacterPhysicsComponent : ClientPhysicsComponent
{
    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        return base.TrySync(parameterName, bitStream);
    }
}