using Catalog.Data;
using Catalog.Data.Repositories;
using Catalog.Domain.Handlers;
using Catalog.Domain.Interfaces.Handlers;
using Catalog.Domain.Interfaces.Repositories;
using Catalog.Minimal.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            //sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });
});

builder.Services.AddScoped<ICatalogApiRepository, CatalogApiRepository>();
builder.Services.AddScoped<ICatalogApiHandler, CatalogApiHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/swagger/v1/swagger.json")
               .WithTitle("Catalog API")
               .WithTheme(ScalarTheme.Solarized)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.MapGroup("/api/catalog")
   .MapHealthEndpoint()
   .MapCatalogApiEndpoints()
   .WithOpenApi();

app.Run();

public partial class Program { }