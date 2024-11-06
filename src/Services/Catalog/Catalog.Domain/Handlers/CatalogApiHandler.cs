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

    public Task<IEnumerable<Plate>> GetPlatesAsync()
    {
        return _catalogApiRepository.GetPlatesAsync();
    }
}
