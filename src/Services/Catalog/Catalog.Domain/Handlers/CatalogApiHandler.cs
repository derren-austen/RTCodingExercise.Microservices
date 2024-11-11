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
}
