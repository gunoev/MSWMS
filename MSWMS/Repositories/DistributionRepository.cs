using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Entities.Distributions;
using MSWMS.Interfaces;
using MSWMS.Repositories.Interfaces;

namespace MSWMS.Repositories;

public class DistributionRepository : IDistributionRepository
{
    private readonly AppDbContext _context;

    public DistributionRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Distribution?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Distributions.FindAsync([id], cancellationToken: cancellationToken);
    }
    
    public async Task<IEnumerable<Distribution>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Distributions.ToListAsync(cancellationToken);
    }
    
    public async Task<Distribution> AddAsync(Distribution entity, CancellationToken cancellationToken = default)
    {
        await _context.Distributions.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }
    
    public async Task UpdateAsync(Distribution entity, CancellationToken cancellationToken = default)
    {
        _context.Distributions.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var distribution = await _context.Distributions.FindAsync([id], cancellationToken: cancellationToken);
        if (distribution != null)
        {
            _context.Distributions.Remove(distribution);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
    
    public async Task<ICollection<Distribution>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        return await _context.Distributions.Where(d => d.Date >= startDate && d.Date <= endDate).ToListAsync();
    }
    
    public async Task<ICollection<DistributionScan>> GetScansByDistributionIdAsync(int distributionId, CancellationToken cancellationToken = default)
    {
        return await _context.DistributionScans
            .Include(ds => ds.User)
            .Where(ds => ds.DistributionId == distributionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<DistributionItem>> GetItemsByDistributionIdAsync(int distributionId,
        CancellationToken cancellationToken = default)
    {
        var documentsIds = await _context.DistributionDocuments
            .Where(dd => dd.DistributionId == distributionId)
            .Select(dd => dd.Id)
            .ToListAsync(cancellationToken);
        
        return await _context.DistributionItems
            .Include(di => di.Destination)
            .Where(di => documentsIds.Contains(di.DocumentId))
            .ToListAsync(cancellationToken);
    }

}