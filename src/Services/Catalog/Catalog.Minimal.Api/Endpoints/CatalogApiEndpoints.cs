﻿using Ardalis.Result.AspNetCore;

using Catalog.Domain.Interfaces.Handlers;

namespace Catalog.Minimal.Api.Endpoints;

public static class CatalogApiEndpoints
{
    public static RouteGroupBuilder MapCatalogApiEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapGet("/plates", GetPlates)
              .WithName("GetPlates");

        //routes.MapGet("/plates", GetPlates) etc
        //      .WithName("GetPlates");

        return routes;
    }

    public static async Task<IResult> GetPlates(ICatalogApiHandler catalogApiHandler)
    {
        var result = await catalogApiHandler.GetPlatesAsync();
        return result.ToMinimalApiResult();
    }
}
