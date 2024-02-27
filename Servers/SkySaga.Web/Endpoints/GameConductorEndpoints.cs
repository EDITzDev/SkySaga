using System;

using Microsoft.AspNetCore.Builder;

namespace SkySaga.Web.Endpoints;

public static class GameConductorEndpoints
{
    public record GameConductorReserve(int Character, Guid ImUuid);

    public static void MapGameConductorEndpoints(this WebApplication app)
    {
        app.MapGet("/api/game-conductor/geonode", () => new
        {
            result = new[]
            {
                new
                {
                    uuid = Guid.NewGuid(),
                    datacentre = "UK",
                    ip = "127.0.0.1",
                    port = 5164
                }
            }
        });

        app.MapPut("/api/game-conductor/reserve", (GameConductorReserve reserve) => new
        {
            result = new
            {
            }
        });

        app.MapPost("/api/game-conductor/retrieve", () => new
        {
            result = new
            {
                retryInMillis = 5000,
                world = Guid.NewGuid(),
                ip = "127.0.0.1",
                port = 42069,
                server = Guid.NewGuid()
            }
        });
    }
}