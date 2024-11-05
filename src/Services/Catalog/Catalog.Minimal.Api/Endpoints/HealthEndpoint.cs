namespace Catalog.Minimal.Api.Endpoints;

public static class HealthEndpoint
{
    public static RouteGroupBuilder MapHealthEndpoint(this RouteGroupBuilder routes)
    {
        routes.MapGet("/health", GetApiHealth)
              .WithName("GetCatalogApiHealth");

        return routes;
    }

    public static IResult GetApiHealth() => Results.Ok("Catalog API is healthy!");
}
