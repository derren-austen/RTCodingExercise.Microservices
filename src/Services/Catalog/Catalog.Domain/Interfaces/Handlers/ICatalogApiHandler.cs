using Ardalis.Result;

using Catalog.Domain.Models;

namespace Catalog.Domain.Interfaces.Handlers;

public interface ICatalogApiHandler
{
    Task<Result<IEnumerable<Plate>>> GetPlatesAsync();
}
