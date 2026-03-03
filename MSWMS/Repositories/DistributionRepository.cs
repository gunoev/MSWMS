using System.Data.Entity;
using MSWMS.Entities;
using MSWMS.Entities.Distributions;
using MSWMS.Interfaces;

namespace MSWMS.Repositories;

public class DistributionRepository : IAsyncRepository<Distribution>
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
    
}