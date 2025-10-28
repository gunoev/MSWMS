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
    public DbSet<Location> Locations { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShipmentEvent> ShipmentEvents { get; set; }
    
    public AppDbContext(DbContextOptionsBuilder? optionsBuilder = null)
    {
        if (optionsBuilder is not null)
        {
            OnConfiguring(optionsBuilder);
        }

        //Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=mswms.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // USER RELATIONSHIPS
        modelBuilder.Entity<User>()
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users);
        
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasOne(u => u.Location);
        
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
            .WithMany(x => x.Scans)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Scan>()
            .HasOne(x => x.User)
            .WithMany();

        modelBuilder.Entity<Scan>()
            .HasIndex(s => s.TimeStamp);
        
        // ORDER RELATIONSHIPS
        modelBuilder.Entity<Order>()
            .HasMany(x => x.Items);
        
        modelBuilder.Entity<Order>()
            .HasMany(x => x.Boxes)
            .WithOne(x => x.Order);
        
        modelBuilder.Entity<Order>()
            .HasMany(x => x.Scans)
            .WithOne(x => x.Order);

        modelBuilder.Entity<Order>()
            .HasMany(x => x.Users);

        modelBuilder.Entity<Order>()
            .HasMany(x => x.Shipments)
            .WithMany(s => s.Orders);

        // ITEM RELATIONSHIPS
        modelBuilder.Entity<Item>()
            .HasMany(x => x.ItemInfo)
            .WithMany();
        
        // ITEM INFO RELATIONSHIPS
        modelBuilder.Entity<ItemInfo>()
            .HasIndex(inf => inf.ItemNumber);
        modelBuilder.Entity<ItemInfo>()
            .HasIndex(inf => inf.Variant);
        modelBuilder.Entity<ItemInfo>()
            .HasIndex(inf => inf.Barcode);
        
        // LOCATIONS RELATIONSHIPS
        modelBuilder.Entity<Location>()
            .HasMany(o => o.OriginOrders).WithOne();
        
        modelBuilder.Entity<Location>()
            .HasMany(o => o.DestinationOrders).WithOne();
        
        // SHIPMENTS RELATIONSHIPS
        modelBuilder.Entity<Shipment>()
            .HasMany(s => s.Events)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Shipment>()
            .HasOne(s => s.Destination);

        modelBuilder.Entity<Shipment>()
            .HasOne(s => s.CreatedBy)
            .WithMany();

        modelBuilder.Entity<Shipment>()
            .HasIndex(s => s.CreatedAt);
        
        modelBuilder.Entity<Shipment>()
            .HasIndex(s => s.Scheduled);
        
        // SHIPMENT EVENT RELATIONSHIPS
        modelBuilder.Entity<ShipmentEvent>()
            .HasOne(e => e.Location)
            .WithMany();
        
        modelBuilder.Entity<ShipmentEvent>()
            .HasOne(e => e.Box)
            .WithMany();

        modelBuilder.Entity<ShipmentEvent>()
            .HasOne(s => s.User)
            .WithMany();
        
        modelBuilder.Entity<ShipmentEvent>()
            .HasIndex(e => e.Timestamp);

        modelBuilder.Entity<ShipmentEvent>()
            .HasIndex(e => e.Code);
        
        // BOXES INDEXES
        
        modelBuilder.Entity<Box>()
            .HasIndex(b => b.UniqueId);

        modelBuilder.Entity<Box>()
            .HasIndex(b => b.Guid);

    }

}