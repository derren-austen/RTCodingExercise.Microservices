using Ardalis.Result;

using Catalog.Domain.Interfaces.Handlers;
using Catalog.Domain.Interfaces.Repositories;
using Catalog.Domain.Models;

namespace Catalog.Domain.Handlers;

public class CatalogApiHandler : ICatalogApiHandler
{
    private readonly ICatalogApiRepository _catalogApiRepository;

    public CatalogApiHandler(ICatalogApiRepository catalogApiRepository)
    {
        _catalogApiRepository = catalogApiRepository;
    }

    public async Task<Result<IEnumerable<Plate>>> GetPlatesAsync(int platesPerPage, int page)
    {
        var plates = await _catalogApiRepository.GetPlatesAsync(platesPerPage, page);
        return plates.Any()
            ? Result<IEnumerable<Plate>>.Success(plates)
            : Result<IEnumerable<Plate>>.NotFound();
    }

    public async Task<Result<Plate>> CreatePlateAsync(Plate plate)
    {
        if (!IsSalePriceCorrectlyMarkedUp(plate))
            return Result<Plate>.Error("Sale price must be at least 20% higher than the purchase price");

        return await _catalogApiRepository.CreatePlateAsync(plate)
            ? Result<Plate>.Created(plate)
            : Result<Plate>.Error("Error creating plate");
    }

    private static bool IsSalePriceCorrectlyMarkedUp(Plate plate)
        => plate.SalePrice >= plate.PurchasePrice * 1.2m;

    public async Task<Result<Plate>> GetPlateByIdAsync(Guid id)
    {
        var plate = await _catalogApiRepository.GetPlateByIdAsync(id);
        return plate is not null
            ? Result<Plate>.Success(plate)
            : Result<Plate>.NotFound();
    }
}
