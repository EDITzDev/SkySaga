using System;

using RakNet;

namespace SkySaga.Game.Components;

public class ClientPhysicsComponent : PhysicsComponent
{
    public override bool TrySync(string parameterName, BitStream bitStream)
    {
        return base.TrySync(parameterName, bitStream);
    }
}