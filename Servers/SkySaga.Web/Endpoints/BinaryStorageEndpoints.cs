using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SkySaga.Web.Endpoints;

public static class BinaryStorageEndpoints
{
    public static void MapBinaryStorageEndpoint(this WebApplication app)
    {
        app.MapPost("/api/binary-storage/photos/_whichIsCooler", (int size) =>
        {
            return Results.Ok();
        });
    }
}