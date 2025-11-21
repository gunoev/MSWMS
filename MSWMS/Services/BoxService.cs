using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Models.Responses;

namespace MSWMS.Services;

public class BoxService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    
    public BoxService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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

    public BoxDto EntityToDto(Box box)
    {
        var dto = _mapper.Map<BoxDto>(box);
        dto.HasShipmentEvents = _context.ShipmentEvents.Any(e => e.Box != null && e.Box.Id == box.Id);
        return dto;
    }

    public async Task<Box?> GetBoxByNumberAndOrder(int boxNumber, int orderId)
    {
        try
        {
            return await _context.Boxes
                .Include(b => b.User)
                .Include(b => b.Scans)
                .FirstOrDefaultAsync(b => b.BoxNumber == boxNumber && b.Order.Id == orderId);
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