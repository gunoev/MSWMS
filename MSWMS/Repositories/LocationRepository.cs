using System.Data.Entity;
using MSWMS.Entities;

namespace MSWMS.Repositories;

public class LocationRepository(AppDbContext context)
{
    
    public async Task<Location?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Locations.FindAsync([id], cancellationToken);
    }

    public async Task<Location?> GetByCode(string code, CancellationToken cancellationToken = default)
    {
        return await context.Locations.FirstOrDefaultAsync(l => l.Code == code, cancellationToken);
    }
}