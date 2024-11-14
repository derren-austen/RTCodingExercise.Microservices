using System;
using System.Threading.Tasks;

using Ardalis.Result;

using Catalog.Domain.Handlers;
using Catalog.Domain.Interfaces.Repositories;
using Catalog.Domain.Models;
using Catalog.TestHelpers;

using FluentAssertions;

using NSubstitute;
using NSubstitute.Core;

using Xunit;

namespace Catalog.UnitTests;

public class CatalogApiShould
{
    private ICatalogApiRepository _catalogApiRepositoryMock = Substitute.For<ICatalogApiRepository>();

    [Fact]
    public async Task Return_not_found_when_no_plates_are_found()
    {
        // Arrange

        _catalogApiRepositoryMock.GetPlatesAsync(20, 1).Returns([]);

        // Act

        var sut = new CatalogApiHandler(_catalogApiRepositoryMock);
        var result = await sut.GetPlatesAsync(20, 1);

        // Assert

        result.Status.Should().Be(ResultStatus.NotFound);
    }

    [Fact]
    public async Task Return_okay_when_plates_are_found()
    {
        // Arrange

        _catalogApiRepositoryMock.GetPlatesAsync(20, 1).Returns([new Plate()]);

        // Act

        var sut = new CatalogApiHandler(_catalogApiRepositoryMock);
        var result = await sut.GetPlatesAsync(20, 1);

        // Assert

        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public async Task Return_created_when_a_plate_is_added_successfully()
    {
        // Arrange

        var newPlate = new Plate();
        _catalogApiRepositoryMock.CreatePlateAsync(newPlate).Returns(true);

        // Act

        var sut = new CatalogApiHandler(_catalogApiRepositoryMock);
        var result = await sut.CreatePlateAsync(newPlate);

        // Assert

        result.Status.Should().Be(ResultStatus.Created);
    }

    [Fact]
    public async Task Return_error_when_the_sale_price_of_a_plate_is_less_than_20_percent_of_the_purchase_price()
    {
        // Arrange

        var newPlate = new Plate
        {
            PurchasePrice = 2000,
            SalePrice = 2300
        };
        _catalogApiRepositoryMock.CreatePlateAsync(newPlate).Returns(false);

        // Act

        var sut = new CatalogApiHandler(_catalogApiRepositoryMock);
        var result = await sut.CreatePlateAsync(newPlate);

        // Assert

        result.Status.Should().Be(ResultStatus.Error);
        result.Errors.Should().Contain("Sale price must be at least 20% higher than the purchase price");
    }

    [Fact]
    public async Task Return_error_when_a_plate_is_not_added_successfully()
    {
        // Arrange

        var newPlate = new Plate();
        _catalogApiRepositoryMock.CreatePlateAsync(newPlate).Returns(false);

        // Act

        var sut = new CatalogApiHandler(_catalogApiRepositoryMock);
        var result = await sut.CreatePlateAsync(newPlate);

        // Assert

        result.Status.Should().Be(ResultStatus.Error);
        result.Errors.Should().Contain("Error creating plate");
    }

    [Fact]
    public async Task Return_okay_when_a_given_plate_is_found_successfully()
    {
        // Arrange

        var plateId = Guid.NewGuid();
        _catalogApiRepositoryMock.GetPlateByIdAsync(plateId).Returns(new Plate());

        // Act

        var sut = new CatalogApiHandler(_catalogApiRepositoryMock);
        var result = await sut.GetPlateByIdAsync(plateId);

        // Assert

        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public async Task Return_not_found_when_a_given_plate_is_not_found()
    {
        // Arrange

        var plateId = Guid.NewGuid();
        _catalogApiRepositoryMock.GetPlateByIdAsync(plateId).Returns((Plate?)null);

        // Act

        var sut = new CatalogApiHandler(_catalogApiRepositoryMock);
        var result = await sut.GetPlateByIdAsync(plateId);

        // Assert

        result.Status.Should().Be(ResultStatus.NotFound);
    }
}