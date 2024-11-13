using Ardalis.Result;

using Catalog.Domain.Models;

namespace Catalog.Domain.Interfaces.Handlers;

public interface ICatalogApiHandler
{
    Task<Result<IEnumerable<Plate>>> GetPlatesAsync();

    Task<Result<Plate>> CreatePlateAsync(Plate plate);
    
    Task<Result<Plate>> GetPlateByIdAsync(Guid id);
}
