using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using SkySaga.Game.Packets;
using SkySaga.Game.Entities;

namespace SkySaga.Game.World;

public class Map
{
    private static int _uniqueEntityId = 1;
    private readonly Dictionary<int, Entity> _entities = [];

    public MapDefinition Definition { get; set; }

    public IEnumerable<Entity> Entities => _entities.Values;

    public Map(MapDefinition definition)
    {
        Definition = definition;
    }

    public bool TryGetEntity(int id, [NotNullWhen(true)] out Entity? entity)
    {
        return _entities.TryGetValue(id, out entity);
    }

    public bool TryCreateEntity(string name, [NotNullWhen(true)] out Entity? entity)
    {
        if (!EntityManager.TryCreateEntity(_uniqueEntityId++, name, out entity))
            return false;

        _entities.TryAdd(entity.Id, entity);

        return true;
    }

    public void RemoveEntity(Entity entity)
    {
        _entities.Remove(entity.Id);
    }
}