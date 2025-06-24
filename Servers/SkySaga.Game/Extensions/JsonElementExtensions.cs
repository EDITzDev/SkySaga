using System;
using System.Text.Json;

namespace SkySaga.Game.Extensions;

public static class JsonElementExtensions
{
    public static bool TryGetPropertyIgnoreCase(this JsonElement jsonElement, string propertyName, out JsonElement value)
    {
        foreach (var jsonProperty in jsonElement.EnumerateObject())
        {
            if (string.Equals(jsonProperty.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = jsonProperty.Value;

                return true;
            }
        }

        value = default;

        return false;
    }
}