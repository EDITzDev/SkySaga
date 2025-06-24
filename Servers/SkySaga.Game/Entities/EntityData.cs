using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SkySaga.Game.Entities;

public class EntityData
{
    private Dictionary<(string ComponentName, string ParameterName), int> _syncedParametersByName = new(new EqualityComparer());
    private Dictionary<int, (string ComponentName, string ParameterName)> _syncedParametersByIndex = [];

    private class EqualityComparer : IEqualityComparer<(string ComponentName, string ParameterName)>
    {
        public bool Equals((string ComponentName, string ParameterName) x, (string ComponentName, string ParameterName) y)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(x.ComponentName, y.ComponentName)
                && StringComparer.OrdinalIgnoreCase.Equals(x.ParameterName, y.ParameterName);
        }

        public int GetHashCode([DisallowNull] (string ComponentName, string ParameterName) obj)
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.ComponentName)
                ^ StringComparer.OrdinalIgnoreCase.GetHashCode(obj.ParameterName);
        }
    }

    public string Name { get; init; }
    public int SyncedParametersCount { get; init; }

    public List<ParameterData> Parameters = [];
    public List<ComponentData> Components = [];

    public class ParameterData
    {
        public required string Name { get; init; }

        public int? SyncIndex;
        public JsonElement Value;
    }

    public class ComponentData
    {
        public required string Name { get; init; }

        public Dictionary<string, string> Bindings = [];
    }

    public EntityData(string name, IEnumerable<ParameterData> parameters, IEnumerable<ComponentData> components)
    {
        Name = name;
        SyncedParametersCount = parameters.Count(x => x.SyncIndex.HasValue);

        foreach (var parameter in parameters)
        {
            Parameters.Add(parameter);

            if (parameter.SyncIndex.HasValue)
            {
                foreach (var component in components)
                {
                    foreach (var binding in component.Bindings)
                    {
                        if (binding.Value == parameter.Name)
                        {
                            _syncedParametersByName.TryAdd((component.Name, binding.Key), parameter.SyncIndex.Value);
                            _syncedParametersByIndex.TryAdd(parameter.SyncIndex.Value, (component.Name, binding.Key));
                        }
                    }
                }

            }
        }

        foreach (var component in components)
        {
            Components.Add(component);
        }
    }

    public int GetParameterSyncIndex(string componentName, string parameterName)
    {
        if (!_syncedParametersByName.TryGetValue((componentName, parameterName), out var syncIndex))
            return -1;

        return syncIndex;
    }

    public bool TryGetSyncedParameterInfo(int syncIndex, [NotNullWhen(true)] out string? componentName, [NotNullWhen(true)] out string? parameterName)
    {
        parameterName = null;
        componentName = null;

        if (!_syncedParametersByIndex.TryGetValue(syncIndex, out var parameterData))
            return false;

        parameterName = parameterData.ParameterName;
        componentName = parameterData.ComponentName;

        return true;
    }
}