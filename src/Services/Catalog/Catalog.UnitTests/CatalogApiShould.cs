using System.Threading.Tasks;

using Ardalis.Result;

using Catalog.Domain.Handlers;
using Catalog.Domain.Interfaces.Repositories;
using Catalog.Domain.Models;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Catalog.UnitTests;

public class CatalogApiShould
{
    private ICatalogApiRepository _catalogApiRepositoryMock = Substitute.For<ICatalogApiRepository>();

    [Fact]
    public async Task Return_not_found_when_no_plates_are_found()
    {
        // Arrange

        _catalogApiRepositoryMock.GetPlatesAsync().Returns([]);

        // Act

        var sut = new CatalogApiHandler(_catalogApiRepositoryMock);
        var result = await sut.GetPlatesAsync();

        // Assert

        result.Status.Should().Be(ResultStatus.NotFound);
    }

    [Fact]
    public async Task Return_okay_when_plates_are_found()
    {
        // Arrange

        _catalogApiRepositoryMock.GetPlatesAsync().Returns([new Plate()]);

        // Act

        var sut = new CatalogApiHandler(_catalogApiRepositoryMock);
        var result = await sut.GetPlatesAsync();

        // Assert

        result.Status.Should().Be(ResultStatus.Ok);
    }
}