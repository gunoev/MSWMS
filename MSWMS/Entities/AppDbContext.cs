using Microsoft.EntityFrameworkCore;

namespace MSWMS.Entities;

public class AppDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Box> Boxes { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemInfo> ItemInfos { get; set; }
    public DbSet<Scan> Scans { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public AppDbContext(DbContextOptionsBuilder? optionsBuilder = null)
    {
        if (optionsBuilder is not null)
        {
            OnConfiguring(optionsBuilder);
        }

        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("MSWMS");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // USER RELATIONSHIPS
        modelBuilder.Entity<User>()
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users);
        
        // SCAN RELATIONSHIPS
        modelBuilder.Entity<Scan>()
            .HasOne(x => x.Order)
            .WithMany(x => x.Scans)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Scan>()
            .HasOne(x => x.Item)
            .WithMany();
        
        modelBuilder.Entity<Scan>()
            .HasOne(x => x.Box)
            .WithMany();
        
        modelBuilder.Entity<Scan>()
            .HasOne(x => x.User)
            .WithMany();
        
        // ORDER RELATIONSHIPS
        modelBuilder.Entity<Order>()
            .HasMany(x => x.Items)
            .WithOne(x => x.Order);
        
        modelBuilder.Entity<Order>()
            .HasMany(x => x.Boxes)
            .WithOne(x => x.Order);
        
        modelBuilder.Entity<Order>()
            .HasMany(x => x.Scans)
            .WithOne(x => x.Order);

        modelBuilder.Entity<Order>()
            .HasMany(x => x.Users);

        // ITEM RELATIONSHIPS
        modelBuilder.Entity<Item>()
            .HasMany(x => x.ItemInfo);

        modelBuilder.Entity<Item>()
            .HasOne(x => x.Order).WithMany();
        
    }

}