using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MSWMS.Models;

public partial class DCXWMSContext : DbContext
{
    public DCXWMSContext()
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DCXWMSContext(DbContextOptions<DCXWMSContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public virtual DbSet<DcxMsItemCrossReference> DcxMsItemCrossReference { get; set; }
    
    public override int SaveChanges()
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("This context is read-only.");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_100_CS_AS");

        modelBuilder.Entity<DcxMsItemCrossReference>(entity =>
        {
            entity.HasKey(e => new { e.ItemNo, e.VariantCode, e.UnitOfMeasure, e.CrossReferenceType, e.CrossReferenceTypeNo, e.CrossReferenceNo }).HasName("DCX-MS$Item Cross Reference$0");

            entity.ToTable("DCX-MS$Item Cross Reference");

            entity.HasIndex(e => new { e.CrossReferenceNo, e.ItemNo, e.VariantCode, e.UnitOfMeasure, e.CrossReferenceType, e.CrossReferenceTypeNo }, "$1").IsUnique();

            entity.HasIndex(e => new { e.CrossReferenceNo, e.CrossReferenceType, e.CrossReferenceTypeNo, e.DiscontinueBarCode, e.ItemNo, e.VariantCode, e.UnitOfMeasure }, "$2").IsUnique();

            entity.HasIndex(e => new { e.CrossReferenceType, e.CrossReferenceNo, e.ItemNo, e.VariantCode, e.UnitOfMeasure, e.CrossReferenceTypeNo }, "$3").IsUnique();

            entity.HasIndex(e => new { e.ItemNo, e.VariantCode, e.UnitOfMeasure, e.CrossReferenceType, e.CrossReferenceNo, e.DiscontinueBarCode, e.CrossReferenceTypeNo }, "$4").IsUnique();

            entity.HasIndex(e => new { e.CrossReferenceType, e.CrossReferenceTypeNo, e.ItemNo, e.VariantCode, e.UnitOfMeasure, e.CrossReferenceNo }, "$5").IsUnique();

            entity.Property(e => e.ItemNo)
                .HasMaxLength(20)
                .HasColumnName("Item No_");
            entity.Property(e => e.VariantCode)
                .HasMaxLength(10)
                .HasColumnName("Variant Code");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure");
            entity.Property(e => e.CrossReferenceType).HasColumnName("Cross-Reference Type");
            entity.Property(e => e.CrossReferenceTypeNo)
                .HasMaxLength(30)
                .HasColumnName("Cross-Reference Type No_");
            entity.Property(e => e.CrossReferenceNo)
                .HasMaxLength(20)
                .HasColumnName("Cross-Reference No_");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.DiscontinueBarCode).HasColumnName("Discontinue Bar Code");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
