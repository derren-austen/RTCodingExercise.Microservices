using Catalog.Domain.Models;

namespace Catalog.Domain.Interfaces.Repositories;

public interface ICatalogApiRepository
{
    Task<IEnumerable<Plate>> GetPlatesAsync();
}
