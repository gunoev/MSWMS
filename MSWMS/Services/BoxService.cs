using MSWMS.Entities;

namespace MSWMS.Services;

public class BoxService
{
    public async void AddBox(Box box)
    {
        try
        {
            await using var db = new AppDbContext();
            db.Boxes.Add(box);
            await db.SaveChangesAsync();
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
            await using var db = new AppDbContext();
            db.Boxes.Update(box);
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
            await using var db = new AppDbContext();
            db.Boxes.Remove(box);
            await db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }
    
    public async Task<Box?> GetBoxById(int id)
    {
        await using var db = new AppDbContext();
        return await db.Boxes.FindAsync(id);
    }
}