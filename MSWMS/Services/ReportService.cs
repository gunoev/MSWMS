using MSWMS.Entities;

namespace MSWMS.Services;

public class ReportService
{
    private readonly AppDbContext _context;
    
    public ReportService(AppDbContext context)
    {
        _context = context;
    }
    
    
}