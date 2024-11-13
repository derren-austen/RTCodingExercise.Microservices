using Catalog.Domain.Interfaces.Repositories;
using Catalog.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Data.Repositories;

public class CatalogApiRepository(
    ApplicationDbContext applicationDbContext)
    : ICatalogApiRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<IEnumerable<Plate>> GetPlatesAsync()
    {
        return await _applicationDbContext.Plates.ToListAsync();
    }

    public async Task<bool> CreatePlateAsync(Plate plate)
    {
        try
        {
            _applicationDbContext.Plates.Add(plate);
            return await _applicationDbContext.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<Plate?> GetPlateByIdAsync(Guid id)
    {
        return await _applicationDbContext.Plates.FindAsync(id);
    }
}
