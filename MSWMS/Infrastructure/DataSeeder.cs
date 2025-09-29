using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Services.Interfaces;

namespace MSWMS.Infrastructure;

public static class DataSeeder
{
    public static async Task SeedRoles(AppDbContext dbContext)
    {
        if (!await dbContext.Roles.AnyAsync())
        {
            var roles = Enum.GetValues<Role.RoleType>()
                .Where(r => r != Role.RoleType.Unknown)
                .Select(type => new Role
                {
                    Name = type.ToString(),
                    Type = type,
                    Users = new List<User>()
                });
            
            await dbContext.Roles.AddRangeAsync(roles);
            await dbContext.SaveChangesAsync();
        }
    }
    
    public static async Task SeedAdminUser(AppDbContext dbContext, IAuthService authService)
    {
        if (!await dbContext.Users.AnyAsync(u => u.Username == "admin"))
        {
            // Создание администратора
            var registerModel = new MSWMS.Models.Auth.RegisterModel
            {
                Username = "admin",
                Name = "Administrator",
                Email = "admin@example.com",
                Password = "Admin123!",
                ConfirmPassword = "Admin123!"
            };
            
            var result = await authService.Register(registerModel);
            
            if (result.Success && result.User != null)
            {
                // Добавление роли администратора
                var adminRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Type == Role.RoleType.Admin);
                if (adminRole != null)
                {
                    result.User.Roles.Add(adminRole);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }

    public static async Task SeedDefaultLocation(AppDbContext dbContext)
    {
        if (!await dbContext.Locations.AnyAsync())
        {
            var defaultLocation = new Location
            {
                Name = "Warehouse Mar Roukouz",
                Code = "W01",
                ShortName = "W01"
            };

            dbContext.Locations.Add(defaultLocation);
            await dbContext.SaveChangesAsync();
        }
    }
}