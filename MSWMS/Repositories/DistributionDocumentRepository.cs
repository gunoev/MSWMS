using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Entities.Distributions;
using MSWMS.Interfaces;

namespace MSWMS.Repositories;

public class DistributionDocumentRepository : IAsyncRepository<DistributionDocument>
{
    private readonly AppDbContext _context;

    public DistributionDocumentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DistributionDocument> AddAsync(DistributionDocument distributionDocument, CancellationToken cancellationToken = default)
    {
        _context.DistributionDocuments.Add(distributionDocument);
        await _context.SaveChangesAsync(cancellationToken);
        return distributionDocument;
    }

    public async Task<DistributionDocument?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.DistributionDocuments
            .Include(dd => dd.Items)
            .FirstOrDefaultAsync(dd => dd.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DistributionDocument>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DistributionDocuments
            .Include(dd => dd.Items)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(DistributionDocument distributionDocument, CancellationToken cancellationToken = default)
    {
        var existing = await _context.DistributionDocuments.FindAsync([distributionDocument.Id], cancellationToken);
        if (existing is null)
        {
            throw new InvalidOperationException($"DistributionDocument with Id {distributionDocument.Id} not found.");
        }

        _context.Entry(existing).CurrentValues.SetValues(distributionDocument);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var distributionDocument = await _context.DistributionDocuments.FindAsync([id], cancellationToken: cancellationToken);
        if (distributionDocument is null) return;

        _context.DistributionDocuments.Remove(distributionDocument);
        await _context.SaveChangesAsync(cancellationToken);
    }
}