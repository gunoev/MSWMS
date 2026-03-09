using Microsoft.EntityFrameworkCore;
using MSWMS.Models;
using MSWMS.Repositories.Interfaces;
using MSWMS.TempModels;

namespace MSWMS.Repositories;

public class DcxDistributionRepository : IDcxDistributionRepository
{
    private readonly DCXWMSContext _context;

    public DcxDistributionRepository(DCXWMSContext context)
    {
        _context = context;
    }
    
    public async Task<List<DcxMsWarehouseActivityLine>> GetLinesByDocumentNumber(string documentNumber, int activityType = 2, int actionType = 1, CancellationToken cancellationToken = default)
    {
        var lines = await _context.DcxMsWarehouseActivityLine
            .Where(al => 
                al.No == documentNumber
                && al.ActivityType == activityType
                && al.ActionType == actionType)
            .ToListAsync(cancellationToken);

        return lines;
    }

    public async Task<string> GetTransferDestinationLocationCode(string documentNumber, CancellationToken cancellationToken = default)
    {
        return await _context.DcxMsTransferHeader
            .Where(th => th.No == documentNumber)
            .Select(th => th.TransferToCode)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<DcxMsItemCrossReference> GetItemCrossReference(string barcode)
    {
        return _context.DcxMsItemCrossReference.Where(cr => cr.CrossReferenceNo == barcode).FirstOrDefaultAsync();
    }
    
    public Task<string> GetSalesDestinationLocationCode(string documentNumber)
    {
        throw new NotImplementedException("SO order numbers are not supported for distribution");
    }
}