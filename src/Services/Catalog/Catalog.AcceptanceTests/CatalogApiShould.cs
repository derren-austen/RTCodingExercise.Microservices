using System.Net;
using System.Text.Json;

using Catalog.AcceptanceTests.Helpers;
using Catalog.Data;
using Catalog.Domain.Models;
using Catalog.TestHelpers;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

namespace Catalog.AcceptanceTests;

public class CatalogApiShould :
    IClassFixture<CatalogApiCustomWebApplicationFactory<Program>>,
    IClassFixture<SqlServerTestContainer>
{
    private readonly CatalogApiCustomWebApplicationFactory<Program> _catalogApiTestFactory;
    private readonly SqlServerTestContainer _sqlServerTestContainer;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public CatalogApiShould(
        CatalogApiCustomWebApplicationFactory<Program> catalogApiTestFactory,
        SqlServerTestContainer sqlServerTestContainer)
    {
        _catalogApiTestFactory = catalogApiTestFactory;
        _sqlServerTestContainer = sqlServerTestContainer;

        SetupTestEnvironment();
    }

    [Fact]
    public async Task Be_healthy()
    {
        // Arrange

        var client = _catalogApiTestFactory.CreateClient();

        // Act

        var response = await client.GetAsync("/api/catalog/health");

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Return_list_of_plates()
    {
        // Arrange
        
        var client = _catalogApiTestFactory.CreateClient();
        AddTestPlate();
        AddTestPlate();

        // Act

        var response = await client.GetAsync("/api/catalog/plates");
        var plates = JsonSerializer.Deserialize<IEnumerable<Plate>>(
            await response.Content.ReadAsStringAsync(), _jsonSerializerOptions);

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        plates.Should().NotBeEmpty().And.HaveCount(2);
        plates?.First().PurchasePrice.Should().BePositive();
        plates?.First().SalePrice.Should().BePositive();
        plates?.Last().PurchasePrice.Should().BePositive();
        plates?.Last().SalePrice.Should().BePositive();
    }

    private void SetupTestEnvironment()
    {
        _catalogApiTestFactory.Host = _sqlServerTestContainer.Host;
        _catalogApiTestFactory.Port = _sqlServerTestContainer.Port;
    }

    private void AddTestPlate()
    {
        AddPlateToDatabase(new PlateBuilder());
    }

    private void AddPlateToDatabase(Plate plate)
    {
        var dbContext = GetDbContext();

        dbContext.Plates.Add(plate);
        dbContext.SaveChanges();
    }

    private ApplicationDbContext GetDbContext()
    {
        var scope = _catalogApiTestFactory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    
}
