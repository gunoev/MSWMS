using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Entities.Distributions;
using MSWMS.Repositories;
using MSWMS.Services.Interfaces;

namespace MSWMS.Services;

public class DistributionService : IDistributionService
{
    private readonly DistributionRepository _distributionRepository;
    private readonly AppDbContext _context;

    public DistributionService(DistributionRepository distributionRepository, AppDbContext context)
    {
        _distributionRepository = distributionRepository;
        _context = context;
    }

    public Task<Distribution?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _distributionRepository.GetByIdAsync(id, cancellationToken);
    }

    public Task<IEnumerable<Distribution>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _distributionRepository.GetAllAsync(cancellationToken);
    }

    public Task<Distribution> CreateAsync(Distribution distribution, CancellationToken cancellationToken = default)
    {
        return _distributionRepository.AddAsync(distribution, cancellationToken);
    }

    public async Task<bool> UpdateAsync(Distribution distribution, CancellationToken cancellationToken = default)
    {
        var existingDistribution = await _distributionRepository.GetByIdAsync(distribution.Id, cancellationToken);
        if (existingDistribution is null)
        {
            return false;
        }

        existingDistribution.Date = distribution.Date;
        existingDistribution.Note = distribution.Note;

        await _distributionRepository.UpdateAsync(existingDistribution, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var distribution = await _distributionRepository.GetByIdAsync(id, cancellationToken);
        if (distribution is null)
        {
            return false;
        }

        await _distributionRepository.DeleteAsync(id, cancellationToken);
        return true;
    }

    public async Task<DistributionDocument?> AddDocumentAsync(
        int distributionId,
        string documentNumber,
        string orderNumber,
        int originId,
        int destinationId,
        CancellationToken cancellationToken = default)
    {
        var distribution = await _context.Distributions
            .FirstOrDefaultAsync(d => d.Id == distributionId, cancellationToken);
        if (distribution is null)
        {
            return null;
        }

        var origin = await _context.Locations.FirstOrDefaultAsync(l => l.Id == originId, cancellationToken);
        var destination = await _context.Locations.FirstOrDefaultAsync(l => l.Id == destinationId, cancellationToken);
        if (origin is null || destination is null)
        {
            return null;
        }

        var document = new DistributionDocument
        {
            DocumentNumber = documentNumber,
            OrderNumber = orderNumber,
            OriginId = originId,
            DestinationId = destinationId,
            DistributionId = distributionId,
            Origin = origin,
            Destination = destination,
            Distribution = distribution
        };

        await _context.DistributionDocuments.AddAsync(document, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return document;
    }

    public async Task<bool> RemoveDocumentAsync(int distributionId, int documentId, CancellationToken cancellationToken = default)
    {
        var document = await _context.DistributionDocuments
            .FirstOrDefaultAsync(d => d.Id == documentId && d.DistributionId == distributionId, cancellationToken);
        if (document is null)
        {
            return false;
        }

        _context.DistributionDocuments.Remove(document);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<Distribution?> UpdateNoteAsync(
        int distributionId,
        string note,
        CancellationToken cancellationToken = default)
    {
        var distribution = await _distributionRepository.GetByIdAsync(distributionId, cancellationToken);
        if (distribution is null)
        {
            return null;
        }

        distribution.Note = note;
        await _distributionRepository.UpdateAsync(distribution, cancellationToken);
        return distribution;
    }
}
