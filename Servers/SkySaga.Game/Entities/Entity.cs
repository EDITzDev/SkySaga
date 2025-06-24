using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using RakNet;

using SkySaga.Game.Components;
using SkySaga.Game.Extensions;

namespace SkySaga.Game.Entities;

public class Entity
{
    private readonly BitArray _sync;
    private readonly BitArray _lastSync;
    private readonly EntityData _entityData;

    private readonly Dictionary<string, Component> _components = new(StringComparer.OrdinalIgnoreCase);

    public int Id { get; }

    public string Name => _entityData.Name;

    public bool SyncRequired => _sync.IsEqual(_lastSync) == false;

    public Entity(int id, EntityData entityData, IReadOnlyCollection<Component> components)
    {
        Id = id;

        _entityData = entityData;

        foreach (var component in components)
        {
            component.ParameterChanged += OnComponentParameterChanged;

            _components.Add(component.Name, component);
        }

        _sync = new BitArray(_entityData.SyncedParametersCount);
        _lastSync = new BitArray(_entityData.SyncedParametersCount);
    }

    private void OnComponentParameterChanged(Component component, string parameter)
    {
        var syncIndex = _entityData.GetParameterSyncIndex(component.Name, parameter);

        if (syncIndex > 0)
            _sync.Set(syncIndex, true);
    }

    public bool TryGetComponent(string name, [NotNullWhen(true)] out Component? component)
    {
        return _components.TryGetValue(name, out component);
    }

    public BitStream GetSyncData(bool newEntity)
    {
        var bitArray = new BitArray(_entityData.SyncedParametersCount);

        var parameterBitStream = new BitStream();

        for (var i = 0; i < _entityData.SyncedParametersCount; i++)
        {
            // Skip matching values
            if (!newEntity && _sync[i] == _lastSync[i])
                continue;

            if (!_entityData.TryGetSyncedParameterInfo(i, out var componentName, out var parameterName))
                continue;

            if (!_components.TryGetValue(componentName, out var component))
                continue;

            if (component.TrySync(parameterName, parameterBitStream))
                bitArray.Set(i, true);
        }

        var syncData = new BitStream();

        var data = new byte[16];

        bitArray.CopyTo(data, 0);

        for (int i = 0; i < bitArray.Count / 32; i++)
            Array.Reverse(data, i * 4, 4);

        syncData.WriteBits(data, (uint)bitArray.Count);

        syncData.WriteBits(BitConverter.GetBytes(parameterBitStream.GetNumberOfBitsUsed()), 32 - Util.NumBitsRequiredUInt32(0x20000));

        if (parameterBitStream.GetNumberOfBitsUsed() > 0)
            syncData.Write(parameterBitStream);

        return syncData;
    }
}