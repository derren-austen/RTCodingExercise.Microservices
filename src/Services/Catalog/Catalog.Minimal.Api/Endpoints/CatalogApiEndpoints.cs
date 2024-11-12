using Catalog.Domain.Interfaces.Handlers;
using Catalog.Domain.Messages;

using MassTransit;

namespace Catalog.Minimal.Api.Endpoints;

public static class CatalogApiEndpoints
{
    public static RouteGroupBuilder MapCatalogApiEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapGet("/plates", GetPlates)
              .WithName("GetPlates");

        routes.MapPost("/plates", CreatePlate)
              .WithName("CreatePlate");

        return routes;
    }

    public static async Task<IResult> GetPlates(ICatalogApiHandler catalogApiHandler)
    {
        var result = await catalogApiHandler.GetPlatesAsync();
        return Results.Ok();
    }

    public static async Task<IResult> CreatePlate(
        ICatalogApiHandler catalogApiHandler,
        IPublishEndpoint publishEndpoint)
    {
        var createPlateMessage = new CreatePlateMessage(
            Guid.NewGuid(),
            "LLL123",
            10.0m,
            15.0m,
            "LLL",
            123);

        await publishEndpoint.Publish(createPlateMessage);

        return Results.Accepted();
    }
}
