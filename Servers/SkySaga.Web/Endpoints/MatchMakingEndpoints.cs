using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace SkySaga.Web.Endpoints;

public static class MatchMakingEndpoints
{
    public static void MapMatchMakingEndpoints(this WebApplication app)
    {
        app.MapPost("/api/matchmaking/userdatacentre/create", (string[] dataCentres) =>
        {
            return Results.Ok();
        });
    }
}