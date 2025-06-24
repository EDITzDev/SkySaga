using System;
using System.IO;
using System.Text.Json;
using System.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using SkySaga.Game.Entities;
using SkySaga.Game.Components;
using SkySaga.Game.Extensions;

namespace SkySaga.Game;

public static class EntityManager
{
    private static Dictionary<string, Type> _components = new(StringComparer.OrdinalIgnoreCase);
    private static Dictionary<string, EntityData> _entities = new(StringComparer.OrdinalIgnoreCase);

    static EntityManager()
    {
        LoadEntityData();

        LoadAssemblyComponents();
    }

    private static void LoadAssemblyComponents()
    {
        var componentType = typeof(Component);

        foreach (var assemblyType in componentType.Assembly.GetTypes())
        {
            if (!assemblyType.IsClass
                || assemblyType.IsAbstract
                || !assemblyType.IsSubclassOf(componentType))
                continue;

            _components.Add(assemblyType.Name.ToLower(), assemblyType);
        }
    }

    private static void LoadEntityData()
    {
        using var fileStream = File.OpenRead(@"Data\Entities.json");

        using var jsonDocument = JsonDocument.Parse(fileStream);

        if (!jsonDocument.RootElement.TryGetPropertyIgnoreCase("Entities", out var entitiesElement) &&
            entitiesElement.ValueKind != JsonValueKind.Array)
            throw new InvalidOperationException();

        foreach (var entityElement in entitiesElement.EnumerateArray())
        {
            if (!entityElement.TryGetPropertyIgnoreCase("Name", out var nameElement) ||
                nameElement.ValueKind != JsonValueKind.String)
                throw new InvalidOperationException();

            var entityName = nameElement.GetString();

            ArgumentException.ThrowIfNullOrWhiteSpace(entityName);

            if (!entityElement.TryGetPropertyIgnoreCase("Parameters", out var parametersElement) ||
                parametersElement.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException();

            List<EntityData.ParameterData> parameters = [];

            foreach (var parameterProperty in parametersElement.EnumerateObject())
            {
                var parameterName = parameterProperty.Name;

                var parameterInfo = new EntityData.ParameterData
                {
                    Name = parameterName
                };

                if (parameterProperty.Value.TryGetPropertyIgnoreCase("SyncIndex", out var syncIndexElement))
                {
                    if (!syncIndexElement.TryGetInt32(out int syncIndex))
                        throw new InvalidOperationException();

                    parameterInfo.SyncIndex = syncIndex;
                }

                if (parameterProperty.Value.TryGetPropertyIgnoreCase("Value", out var valueElement))
                {
                    parameterInfo.Value = valueElement;
                }

                parameters.Add(parameterInfo);
            }

            // Client/Server
            if (!entityElement.TryGetPropertyIgnoreCase("Client", out var clientElement) ||
                clientElement.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException();

            if (!clientElement.TryGetPropertyIgnoreCase("Components", out var componentsElement) ||
                componentsElement.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException();

            List<EntityData.ComponentData> components = [];

            foreach (var componentProperty in componentsElement.EnumerateObject())
            {
                var componentName = componentProperty.Name;

                var componentInfo = new EntityData.ComponentData
                {
                    Name = componentName
                };

                if (!componentProperty.Value.TryGetPropertyIgnoreCase("Bindings", out var bindingsElement) ||
                    bindingsElement.ValueKind != JsonValueKind.Object)
                    continue;

                foreach (var bindingProperty in bindingsElement.EnumerateObject())
                {
                    var bindingName = bindingProperty.Name;

                    if (!bindingProperty.Value.TryGetPropertyIgnoreCase("MapsTo", out var mapsToElement) ||
                        mapsToElement.ValueKind != JsonValueKind.String)
                        throw new InvalidOperationException();

                    var mapsTo = mapsToElement.GetString();

                    ArgumentException.ThrowIfNullOrWhiteSpace(mapsTo);

                    componentInfo.Bindings.TryAdd(bindingName, mapsTo);
                }

                components.Add(componentInfo);
            }

            _entities.Add(entityName, new EntityData(entityName, parameters, components));
        }
    }

    public static bool TryCreateEntity(int id, string name, [NotNullWhen(true)] out Entity? entity)
    {
        if (!_entities.TryGetValue(name, out var entityData))
        {
            entity = null;
            return false;
        }

        List<Component> components = new(entityData.Components.Count);

        foreach (var componentData in entityData.Components)
        {
            if (!_components.TryGetValue(componentData.Name, out var componentType))
            {
                Debug.WriteLine(componentData.Name, "Unimplemented Component");
                continue;
            }

            var component = (Component?)Activator.CreateInstance(componentType);

            ArgumentNullException.ThrowIfNull(component);

            components.Add(component);
        }

        entity = new Entity(id, entityData, components);

        return true;
    }
}