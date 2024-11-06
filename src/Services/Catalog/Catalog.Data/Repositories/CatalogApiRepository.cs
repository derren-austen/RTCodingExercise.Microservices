using Catalog.Domain.Interfaces.Repositories;
using Catalog.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Data.Repositories;

public class CatalogApiRepository(ApplicationDbContext applicationDbContext) : ICatalogApiRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<IEnumerable<Plate>> GetPlatesAsync()
    {
        return await _applicationDbContext.Plates.ToListAsync();
    }
}
