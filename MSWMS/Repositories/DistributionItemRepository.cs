using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Entities.Distributions;
using MSWMS.Interfaces;

namespace MSWMS.Repositories;

public class DistributionItemRepository : IAsyncRepository<DistributionItem>
{
    private readonly AppDbContext _context;

    public DistributionItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DistributionItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.DistributionItems
            .FirstOrDefaultAsync(di => di.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DistributionItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DistributionItems.ToListAsync(cancellationToken);
    }

    public async Task<DistributionItem> AddAsync(DistributionItem entity, CancellationToken cancellationToken = default)
    {
        await _context.DistributionItems.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(DistributionItem entity, CancellationToken cancellationToken = default)
    {
        _context.DistributionItems.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.DistributionItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}