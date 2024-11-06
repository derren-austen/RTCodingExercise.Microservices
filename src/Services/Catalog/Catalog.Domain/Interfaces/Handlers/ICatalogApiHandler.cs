using Catalog.Domain.Models;

namespace Catalog.Domain.Interfaces.Handlers;

public interface ICatalogApiHandler
{
    Task<IEnumerable<Plate>> GetPlatesAsync();
}
