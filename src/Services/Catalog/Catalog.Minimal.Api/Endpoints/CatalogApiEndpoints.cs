using Catalog.Domain.Interfaces.Handlers;
using Catalog.Domain.Messages;
using Catalog.Domain.Models;
using Catalog.Minimal.Api.ViewModels;

using MassTransit;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Minimal.Api.Endpoints;

public static class CatalogApiEndpoints
{
    private const string GetPlatesRouteName = "GetPlates";
    private const string GetPlateByIdRouteName = "GetPlateById";
    private const string CreatePlateRouteName = "CreatePlate";

    public static RouteGroupBuilder MapCatalogApiEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapGet("/plates", GetPlates)
              .WithName(GetPlatesRouteName);

        routes.MapGet("/plates/{id:guid}", GetPlateById)
              .WithName(GetPlateByIdRouteName);

        routes.MapPost("/plates", CreatePlate)
              .WithName(CreatePlateRouteName);

        return routes;
    }

    private static async Task<Results<Ok<Plate>, NotFound>> GetPlateById(
        Guid id,
        ICatalogApiHandler catalogApiHandler)
    {
        var result = await catalogApiHandler.GetPlateByIdAsync(id);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }

    public static async Task<Results<Ok<IEnumerable<Plate>>, NotFound>> GetPlates(
        ICatalogApiHandler catalogApiHandler)
    {
        var result = await catalogApiHandler.GetPlatesAsync();
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }

    public static async Task<Results<BadRequest<string>, Accepted>> CreatePlate(
        CreatePlate createPlate,
        IPublishEndpoint publishEndpoint,
        LinkGenerator linkGenerator)
    {
        var validationResult = ValidateCreatePlate(createPlate);
        if (validationResult is not null)
            return validationResult;

        var plateId = Guid.NewGuid();

        var createPlateMessage = new CreatePlateMessage(
            plateId,
            createPlate.Registration,
            createPlate.PurchasePrice,
            createPlate.SalePrice,
            createPlate.Letters,
            createPlate.Numbers);

        await publishEndpoint.Publish(createPlateMessage);

        return TypedResults.Accepted(linkGenerator.GetPathByName(GetPlateByIdRouteName, new { id = plateId }));
    }

    private static Results<BadRequest<string>, Accepted>? ValidateCreatePlate(CreatePlate createPlate)
    {
        if (createPlate.PurchasePrice <= 0)
            return TypedResults.BadRequest("'Purchase price' must be provided");

        if (createPlate.Numbers <= 0)
            return TypedResults.BadRequest("'Numbers' must be provided");

        return null;
    }
}
