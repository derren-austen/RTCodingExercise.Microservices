var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Endpoints

var catalogRoutes =
    app.MapGroup("/api/catalog")
       .WithOpenApi();

catalogRoutes.MapGet("/health", () =>
{
    return Results.Ok("Catalog API is healthy!");
})
.WithName("GetCatalogApiHealth");



#endregion

app.Run();

public partial class Program { }