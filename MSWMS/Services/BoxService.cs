using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;

namespace MSWMS.Services;

public class BoxService
{
    private readonly AppDbContext _context;
    
    public BoxService(AppDbContext context)
    {
        _context = context;
    }
    
    public async void AddBox(Box box)
    {
        try
        {
            _context.Boxes.Add(box);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }
    
    public async void UpdateBox(Box box)
    {
        try
        {
            _context.Boxes.Update(box);
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }
    
    public async void DeleteBox(Box box)
    {
        try
        {
            _context.Boxes.Remove(box);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }

    public async Task<Box?> GetBoxByNumberAndOrder(int boxNumber, int orderId)
    {
        try
        {
            return await _context.Boxes.Include(b => b.User).FirstOrDefaultAsync(b => b.BoxNumber == boxNumber && b.Order.Id == orderId);
        }
        catch (Exception e)
        {
            throw;
        }
    }
    
    public async Task<Box?> GetBoxById(int id)
    {
        return await _context.Boxes.FindAsync(id);
    }
}