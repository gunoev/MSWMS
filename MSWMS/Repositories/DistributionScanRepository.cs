using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Entities.Distributions;
using MSWMS.Interfaces;

namespace MSWMS.Repositories;

public class DistributionScanRepository : IAsyncRepository<DistributionScan>
{
    private readonly AppDbContext _context;

    public DistributionScanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DistributionScan?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.DistributionScans
            .FirstOrDefaultAsync(ds => ds.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DistributionScan>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DistributionScans.ToListAsync(cancellationToken);
    }

    public async Task<DistributionScan> AddAsync(DistributionScan entity, CancellationToken cancellationToken = default)
    {
        await _context.DistributionScans.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(DistributionScan entity, CancellationToken cancellationToken = default)
    {
        _context.DistributionScans.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.DistributionScans.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
