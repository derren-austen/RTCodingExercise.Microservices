using Catalog.Domain.Interfaces.Handlers;
using Catalog.Domain.Messages;
using Catalog.Domain.Models;
using Catalog.Minimal.Api.Config;
using Catalog.Minimal.Api.ViewModels;

using MassTransit;
using MassTransit.Configuration;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

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

    private static async Task<Results<Ok<ViewPlate>, NotFound>> GetPlateById(
        Guid id,
        ICatalogApiHandler catalogApiHandler)
    {
        var result = await catalogApiHandler.GetPlateByIdAsync(id);
        return result.IsSuccess
            ? TypedResults.Ok(MapToViewPlate(result.Value))
            : TypedResults.NotFound();
    }

    private static ViewPlate MapToViewPlate(Plate plate) =>
        new ()
        {
            Id = plate.Id,
            Registration = plate.Registration,
            PurchasePrice = plate.PurchasePrice,
            SalePrice = plate.SalePrice,
            Letters = plate.Letters,
            Numbers = plate.Numbers
        };

    public static async Task<Results<Ok<IEnumerable<ViewPlate>>, NotFound>> GetPlates(
        ICatalogApiHandler catalogApiHandler,
        IOptions<PlateOptions> plateOptions,
        int page = 1)
    {
        var result = await catalogApiHandler.GetPlatesAsync(plateOptions.Value.PlatesPerPage, page);
        return result.IsSuccess
            ? TypedResults.Ok(MapToViewPlates(result.Value))
            : TypedResults.NotFound();
    }

    private static IEnumerable<ViewPlate> MapToViewPlates(IEnumerable<Plate> plates) =>
        plates.Select(MapToViewPlate);

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
