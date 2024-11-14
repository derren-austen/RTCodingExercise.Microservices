using Catalog.Domain.Models;

namespace Catalog.Domain.Interfaces.Repositories;

public interface ICatalogApiRepository
{
    Task<IEnumerable<Plate>> GetPlatesAsync(int platesPerPage, int page);

    Task<bool> CreatePlateAsync(Plate plate);

    Task<Plate?> GetPlateByIdAsync(Guid id);
}
