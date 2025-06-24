using System;
using System.Runtime.CompilerServices;

using RakNet;

namespace SkySaga.Game.Components;

public delegate void ParameterChangedEventHandler(Component component, string parameterName);

public abstract class Component
{
    public string Name => GetType().Name;

    public event ParameterChangedEventHandler? ParameterChanged;

    protected void OnParameterChanged([CallerMemberName] string? parameterName = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(parameterName);

        ParameterChanged?.Invoke(this, parameterName);
    }

    public abstract bool TrySync(string parameterName, BitStream bitStream);
}