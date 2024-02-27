using Microsoft.AspNetCore.Builder;

namespace SkySaga.Web.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this WebApplication app)
    {
        app.MapPost("/api/account/get", () => new
        {
            result = new
            {
                keySubset = new
                {
                    RESERVED_NAME = "EDITz"
                }
            }
        });
    }
}