using System.Net;

using Catalog.AcceptanceTests.Helpers;

using FluentAssertions;

namespace Catalog.AcceptanceTests;

public class CatalogApiShould(CatalogApiClassFixture catalogApiClassFixture) : IClassFixture<CatalogApiClassFixture>
{
    private readonly CatalogApiClassFixture _catalogApiClassFixture = catalogApiClassFixture;

    [Fact]
    public async Task Catalog_Api_should_be_healthy()
    {
        // Arrange

        var client = _catalogApiClassFixture.CreateClient();

        // Act

        var response = await client.GetAsync("/api/catalog/health");

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}