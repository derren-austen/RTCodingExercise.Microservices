using Catalog.Data;
using Catalog.Data.Repositories;
using Catalog.Domain.Handlers;
using Catalog.Domain.Interfaces.Handlers;
using Catalog.Domain.Interfaces.Repositories;
using Catalog.Minimal.Api.Config;
using Catalog.Minimal.Api.Consumers;
using Catalog.Minimal.Api.Endpoints;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddOptions<PlateOptions>()
    .BindConfiguration(PlateOptions.PlatesConfig);

#region EF config

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });
});

#endregion

#region MassTransit config

var eventBusSettings = builder.Configuration
    .GetSection(EventBusOptions.EventBusConfig)
    .Get<EventBusOptions>();

builder.Services.AddMassTransit(rc =>
{
    rc.AddConsumer<CreatePlateConsumer>();

    rc.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(eventBusSettings!.Connection, "/", hc =>
        {
            hc.Username(eventBusSettings!.Username);
            hc.Password(eventBusSettings!.Password);
        });

        cfg.ReceiveEndpoint(eventBusSettings!.CreatePlateQueueName, ec =>
        {
            ec.ConfigureConsumer<CreatePlateConsumer>(ctx);
        });
    });
});

#endregion

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