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

    public async Task<Result<IEnumerable<Plate>>> GetPlatesAsync()
    {
        var plates = await _catalogApiRepository.GetPlatesAsync();
        return plates.Any()
            ? Result<IEnumerable<Plate>>.Success(plates)
            : Result<IEnumerable<Plate>>.NotFound();
    }

    public async Task<Result<Plate>> CreatePlateAsync(Plate plate)
    {
        return await _catalogApiRepository.CreatePlateAsync(plate)
            ? Result<Plate>.Created(plate)
            : Result<Plate>.Error("Error creating plate");
    }

    public async Task<Result<Plate>> GetPlateByIdAsync(Guid id)
    {
        var plate = await _catalogApiRepository.GetPlateByIdAsync(id);
        return plate is not null
            ? Result<Plate>.Success(plate)
            : Result<Plate>.NotFound();
    }
}
