using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MSWMS.TempModels;

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
    public virtual DbSet<DcxMsWarehouseActivityLine> DcxMsWarehouseActivityLine { get; set; }
    public virtual DbSet<DcxMsWarehouseActivityHeader> DcxMsWarehouseActivityHeader { get; set; }
    public virtual DbSet<DcxMsSalesHeader> DcxMsSalesHeader { get; set; }
    public virtual DbSet<DcxMsTransferHeader> DcxMsTransferHeader { get; set; }
    
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
        
        modelBuilder.Entity<DcxMsSalesHeader>(entity =>
        {
            entity.HasKey(e => new { e.DocumentType, e.No }).HasName("DCX-MS$Sales Header$0");

            entity.ToTable("DCX-MS$Sales Header");

            entity.HasIndex(e => new { e.No, e.DocumentType }, "$1").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.SellToCustomerNo, e.No }, "$2").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.BillToCustomerNo, e.No }, "$3").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.CombineShipments, e.BillToCustomerNo, e.CurrencyCode, e.Eu3PartyTrade, e.DimensionSetId, e.No }, "$4").IsUnique();

            entity.HasIndex(e => new { e.SellToCustomerNo, e.ExternalDocumentNo, e.DocumentType, e.No }, "$5").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.SellToContactNo, e.No }, "$6").IsUnique();

            entity.HasIndex(e => new { e.BillToContactNo, e.DocumentType, e.No }, "$7").IsUnique();

            entity.HasIndex(e => new { e.IncomingDocumentEntryNo, e.DocumentType, e.No }, "$8").IsUnique();

            entity.Property(e => e.DocumentType).HasColumnName("Document Type");
            entity.Property(e => e.No)
                .HasMaxLength(20)
                .HasColumnName("No_");
            entity.Property(e => e.AllowLineDisc).HasColumnName("Allow Line Disc_");
            entity.Property(e => e.AppliesToDocNo)
                .HasMaxLength(20)
                .HasColumnName("Applies-to Doc_ No_");
            entity.Property(e => e.AppliesToDocType).HasColumnName("Applies-to Doc_ Type");
            entity.Property(e => e.AppliesToId)
                .HasMaxLength(50)
                .HasColumnName("Applies-to ID");
            entity.Property(e => e.Area).HasMaxLength(10);
            entity.Property(e => e.AssignedUserId)
                .HasMaxLength(50)
                .HasColumnName("Assigned User ID");
            entity.Property(e => e.AuthorizationRequired).HasColumnName("Authorization Required");
            entity.Property(e => e.BalAccountNo)
                .HasMaxLength(20)
                .HasColumnName("Bal_ Account No_");
            entity.Property(e => e.BalAccountType).HasColumnName("Bal_ Account Type");
            entity.Property(e => e.BillToAddress)
                .HasMaxLength(50)
                .HasColumnName("Bill-to Address");
            entity.Property(e => e.BillToAddress2)
                .HasMaxLength(50)
                .HasColumnName("Bill-to Address 2");
            entity.Property(e => e.BillToCity)
                .HasMaxLength(30)
                .HasColumnName("Bill-to City");
            entity.Property(e => e.BillToContact)
                .HasMaxLength(50)
                .HasColumnName("Bill-to Contact");
            entity.Property(e => e.BillToContactNo)
                .HasMaxLength(20)
                .HasColumnName("Bill-to Contact No_");
            entity.Property(e => e.BillToCountryRegionCode)
                .HasMaxLength(10)
                .HasColumnName("Bill-to Country_Region Code");
            entity.Property(e => e.BillToCounty)
                .HasMaxLength(30)
                .HasColumnName("Bill-to County");
            entity.Property(e => e.BillToCustomerNo)
                .HasMaxLength(20)
                .HasColumnName("Bill-to Customer No_");
            entity.Property(e => e.BillToCustomerTemplateCode)
                .HasMaxLength(10)
                .HasColumnName("Bill-to Customer Template Code");
            entity.Property(e => e.BillToIcPartnerCode)
                .HasMaxLength(20)
                .HasColumnName("Bill-to IC Partner Code");
            entity.Property(e => e.BillToName)
                .HasMaxLength(50)
                .HasColumnName("Bill-to Name");
            entity.Property(e => e.BillToName2)
                .HasMaxLength(50)
                .HasColumnName("Bill-to Name 2");
            entity.Property(e => e.BillToPostCode)
                .HasMaxLength(20)
                .HasColumnName("Bill-to Post Code");
            entity.Property(e => e.CampaignNo)
                .HasMaxLength(20)
                .HasColumnName("Campaign No_");
            entity.Property(e => e.CombineShipments).HasColumnName("Combine Shipments");
            entity.Property(e => e.CompressPrepayment).HasColumnName("Compress Prepayment");
            entity.Property(e => e.CreditCardNo)
                .HasMaxLength(20)
                .HasColumnName("Credit Card No_");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasColumnName("Currency Code");
            entity.Property(e => e.CurrencyFactor)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Currency Factor");
            entity.Property(e => e.CustomerDiscGroup)
                .HasMaxLength(20)
                .HasColumnName("Customer Disc_ Group");
            entity.Property(e => e.CustomerPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Customer Posting Group");
            entity.Property(e => e.CustomerPriceGroup)
                .HasMaxLength(10)
                .HasColumnName("Customer Price Group");
            entity.Property(e => e.DimensionSetId).HasColumnName("Dimension Set ID");
            entity.Property(e => e.DirectDebitMandateId)
                .HasMaxLength(35)
                .HasColumnName("Direct Debit Mandate ID");
            entity.Property(e => e.DocNoOccurrence).HasColumnName("Doc_ No_ Occurrence");
            entity.Property(e => e.DocumentDate)
                .HasColumnType("datetime")
                .HasColumnName("Document Date");
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("Due Date");
            entity.Property(e => e.Eu3PartyTrade).HasColumnName("EU 3-Party Trade");
            entity.Property(e => e.ExitPoint)
                .HasMaxLength(10)
                .HasColumnName("Exit Point");
            entity.Property(e => e.ExternalDocumentNo)
                .HasMaxLength(35)
                .HasColumnName("External Document No_");
            entity.Property(e => e.GenBusPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Gen_ Bus_ Posting Group");
            entity.Property(e => e.GetShipmentUsed).HasColumnName("Get Shipment Used");
            entity.Property(e => e.IcDirection).HasColumnName("IC Direction");
            entity.Property(e => e.IcStatus).HasColumnName("IC Status");
            entity.Property(e => e.IncomingDocumentEntryNo).HasColumnName("Incoming Document Entry No_");
            entity.Property(e => e.InvoiceDiscCode)
                .HasMaxLength(20)
                .HasColumnName("Invoice Disc_ Code");
            entity.Property(e => e.InvoiceDiscountCalculation).HasColumnName("Invoice Discount Calculation");
            entity.Property(e => e.InvoiceDiscountValue)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Invoice Discount Value");
            entity.Property(e => e.JobQueueEntryId).HasColumnName("Job Queue Entry ID");
            entity.Property(e => e.JobQueueStatus).HasColumnName("Job Queue Status");
            entity.Property(e => e.LanguageCode)
                .HasMaxLength(10)
                .HasColumnName("Language Code");
            entity.Property(e => e.LastPostingNo)
                .HasMaxLength(20)
                .HasColumnName("Last Posting No_");
            entity.Property(e => e.LastPrepaymentNo)
                .HasMaxLength(20)
                .HasColumnName("Last Prepayment No_");
            entity.Property(e => e.LastPrepmtCrMemoNo)
                .HasMaxLength(20)
                .HasColumnName("Last Prepmt_ Cr_ Memo No_");
            entity.Property(e => e.LastReturnReceiptNo)
                .HasMaxLength(20)
                .HasColumnName("Last Return Receipt No_");
            entity.Property(e => e.LastShippingNo)
                .HasMaxLength(20)
                .HasColumnName("Last Shipping No_");
            entity.Property(e => e.LocationCode)
                .HasMaxLength(10)
                .HasColumnName("Location Code");
            entity.Property(e => e.NoPrinted).HasColumnName("No_ Printed");
            entity.Property(e => e.NoSeries)
                .HasMaxLength(10)
                .HasColumnName("No_ Series");
            entity.Property(e => e.OnHold)
                .HasMaxLength(3)
                .HasColumnName("On Hold");
            entity.Property(e => e.OpportunityNo)
                .HasMaxLength(20)
                .HasColumnName("Opportunity No_");
            entity.Property(e => e.OrderClass)
                .HasMaxLength(10)
                .HasColumnName("Order Class");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("Order Date");
            entity.Property(e => e.OutboundWhseHandlingTime)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Outbound Whse_ Handling Time");
            entity.Property(e => e.PackageTrackingNo)
                .HasMaxLength(30)
                .HasColumnName("Package Tracking No_");
            entity.Property(e => e.PaymentDiscount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Payment Discount _");
            entity.Property(e => e.PaymentMethodCode)
                .HasMaxLength(10)
                .HasColumnName("Payment Method Code");
            entity.Property(e => e.PaymentTermsCode)
                .HasMaxLength(10)
                .HasColumnName("Payment Terms Code");
            entity.Property(e => e.PmtDiscountDate)
                .HasColumnType("datetime")
                .HasColumnName("Pmt_ Discount Date");
            entity.Property(e => e.PostingDate)
                .HasColumnType("datetime")
                .HasColumnName("Posting Date");
            entity.Property(e => e.PostingDescription)
                .HasMaxLength(50)
                .HasColumnName("Posting Description");
            entity.Property(e => e.PostingFromWhseRef).HasColumnName("Posting from Whse_ Ref_");
            entity.Property(e => e.PostingNo)
                .HasMaxLength(20)
                .HasColumnName("Posting No_");
            entity.Property(e => e.PostingNoSeries)
                .HasMaxLength(10)
                .HasColumnName("Posting No_ Series");
            entity.Property(e => e.Prepayment)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepayment _");
            entity.Property(e => e.PrepaymentDueDate)
                .HasColumnType("datetime")
                .HasColumnName("Prepayment Due Date");
            entity.Property(e => e.PrepaymentNo)
                .HasMaxLength(20)
                .HasColumnName("Prepayment No_");
            entity.Property(e => e.PrepaymentNoSeries)
                .HasMaxLength(10)
                .HasColumnName("Prepayment No_ Series");
            entity.Property(e => e.PrepmtCrMemoNo)
                .HasMaxLength(20)
                .HasColumnName("Prepmt_ Cr_ Memo No_");
            entity.Property(e => e.PrepmtCrMemoNoSeries)
                .HasMaxLength(10)
                .HasColumnName("Prepmt_ Cr_ Memo No_ Series");
            entity.Property(e => e.PrepmtPaymentDiscount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt_ Payment Discount _");
            entity.Property(e => e.PrepmtPaymentTermsCode)
                .HasMaxLength(10)
                .HasColumnName("Prepmt_ Payment Terms Code");
            entity.Property(e => e.PrepmtPmtDiscountDate)
                .HasColumnType("datetime")
                .HasColumnName("Prepmt_ Pmt_ Discount Date");
            entity.Property(e => e.PrepmtPostingDescription)
                .HasMaxLength(50)
                .HasColumnName("Prepmt_ Posting Description");
            entity.Property(e => e.PricesIncludingVat).HasColumnName("Prices Including VAT");
            entity.Property(e => e.PrintPostedDocuments).HasColumnName("Print Posted Documents");
            entity.Property(e => e.PromisedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Promised Delivery Date");
            entity.Property(e => e.QuoteNo)
                .HasMaxLength(20)
                .HasColumnName("Quote No_");
            entity.Property(e => e.ReasonCode)
                .HasMaxLength(10)
                .HasColumnName("Reason Code");
            entity.Property(e => e.RequestedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Requested Delivery Date");
            entity.Property(e => e.ResponsibilityCenter)
                .HasMaxLength(10)
                .HasColumnName("Responsibility Center");
            entity.Property(e => e.ReturnReceiptNo)
                .HasMaxLength(20)
                .HasColumnName("Return Receipt No_");
            entity.Property(e => e.ReturnReceiptNoSeries)
                .HasMaxLength(10)
                .HasColumnName("Return Receipt No_ Series");
            entity.Property(e => e.SalespersonCode)
                .HasMaxLength(10)
                .HasColumnName("Salesperson Code");
            entity.Property(e => e.SellToAddress)
                .HasMaxLength(50)
                .HasColumnName("Sell-to Address");
            entity.Property(e => e.SellToAddress2)
                .HasMaxLength(50)
                .HasColumnName("Sell-to Address 2");
            entity.Property(e => e.SellToCity)
                .HasMaxLength(30)
                .HasColumnName("Sell-to City");
            entity.Property(e => e.SellToContact)
                .HasMaxLength(50)
                .HasColumnName("Sell-to Contact");
            entity.Property(e => e.SellToContactNo)
                .HasMaxLength(20)
                .HasColumnName("Sell-to Contact No_");
            entity.Property(e => e.SellToCountryRegionCode)
                .HasMaxLength(10)
                .HasColumnName("Sell-to Country_Region Code");
            entity.Property(e => e.SellToCounty)
                .HasMaxLength(30)
                .HasColumnName("Sell-to County");
            entity.Property(e => e.SellToCustomerName)
                .HasMaxLength(50)
                .HasColumnName("Sell-to Customer Name");
            entity.Property(e => e.SellToCustomerName2)
                .HasMaxLength(50)
                .HasColumnName("Sell-to Customer Name 2");
            entity.Property(e => e.SellToCustomerNo)
                .HasMaxLength(20)
                .HasColumnName("Sell-to Customer No_");
            entity.Property(e => e.SellToCustomerTemplateCode)
                .HasMaxLength(10)
                .HasColumnName("Sell-to Customer Template Code");
            entity.Property(e => e.SellToIcPartnerCode)
                .HasMaxLength(20)
                .HasColumnName("Sell-to IC Partner Code");
            entity.Property(e => e.SellToPostCode)
                .HasMaxLength(20)
                .HasColumnName("Sell-to Post Code");
            entity.Property(e => e.SendIcDocument).HasColumnName("Send IC Document");
            entity.Property(e => e.ShipToAddress)
                .HasMaxLength(50)
                .HasColumnName("Ship-to Address");
            entity.Property(e => e.ShipToAddress2)
                .HasMaxLength(50)
                .HasColumnName("Ship-to Address 2");
            entity.Property(e => e.ShipToCity)
                .HasMaxLength(30)
                .HasColumnName("Ship-to City");
            entity.Property(e => e.ShipToCode)
                .HasMaxLength(10)
                .HasColumnName("Ship-to Code");
            entity.Property(e => e.ShipToContact)
                .HasMaxLength(50)
                .HasColumnName("Ship-to Contact");
            entity.Property(e => e.ShipToCountryRegionCode)
                .HasMaxLength(10)
                .HasColumnName("Ship-to Country_Region Code");
            entity.Property(e => e.ShipToCounty)
                .HasMaxLength(30)
                .HasColumnName("Ship-to County");
            entity.Property(e => e.ShipToName)
                .HasMaxLength(50)
                .HasColumnName("Ship-to Name");
            entity.Property(e => e.ShipToName2)
                .HasMaxLength(50)
                .HasColumnName("Ship-to Name 2");
            entity.Property(e => e.ShipToPostCode)
                .HasMaxLength(20)
                .HasColumnName("Ship-to Post Code");
            entity.Property(e => e.ShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Shipment Date");
            entity.Property(e => e.ShipmentMethodCode)
                .HasMaxLength(10)
                .HasColumnName("Shipment Method Code");
            entity.Property(e => e.ShippingAdvice).HasColumnName("Shipping Advice");
            entity.Property(e => e.ShippingAgentCode)
                .HasMaxLength(10)
                .HasColumnName("Shipping Agent Code");
            entity.Property(e => e.ShippingAgentServiceCode)
                .HasMaxLength(10)
                .HasColumnName("Shipping Agent Service Code");
            entity.Property(e => e.ShippingNo)
                .HasMaxLength(20)
                .HasColumnName("Shipping No_");
            entity.Property(e => e.ShippingNoSeries)
                .HasMaxLength(10)
                .HasColumnName("Shipping No_ Series");
            entity.Property(e => e.ShippingTime)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Shipping Time");
            entity.Property(e => e.ShortcutDimension1Code)
                .HasMaxLength(20)
                .HasColumnName("Shortcut Dimension 1 Code");
            entity.Property(e => e.ShortcutDimension2Code)
                .HasMaxLength(20)
                .HasColumnName("Shortcut Dimension 2 Code");
            entity.Property(e => e.SourceDocumentNo)
                .HasMaxLength(20)
                .HasColumnName("Source Document No_");
            entity.Property(e => e.TaxAreaCode)
                .HasMaxLength(20)
                .HasColumnName("Tax Area Code");
            entity.Property(e => e.TaxLiable).HasColumnName("Tax Liable");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TransactionSpecification)
                .HasMaxLength(10)
                .HasColumnName("Transaction Specification");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(10)
                .HasColumnName("Transaction Type");
            entity.Property(e => e.TransportMethod)
                .HasMaxLength(10)
                .HasColumnName("Transport Method");
            entity.Property(e => e.VatBaseDiscount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("VAT Base Discount _");
            entity.Property(e => e.VatBusPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("VAT Bus_ Posting Group");
            entity.Property(e => e.VatCountryRegionCode)
                .HasMaxLength(10)
                .HasColumnName("VAT Country_Region Code");
            entity.Property(e => e.VatRegistrationNo)
                .HasMaxLength(20)
                .HasColumnName("VAT Registration No_");
            entity.Property(e => e.YourReference)
                .HasMaxLength(35)
                .HasColumnName("Your Reference");
        });

        modelBuilder.Entity<DcxMsTransferHeader>(entity =>
        {
            entity.HasKey(e => e.No).HasName("DCX-MS$Transfer Header$0");

            entity.ToTable("DCX-MS$Transfer Header");

            entity.Property(e => e.No)
                .HasMaxLength(20)
                .HasColumnName("No_");
            entity.Property(e => e.Area).HasMaxLength(10);
            entity.Property(e => e.AssignedUserId)
                .HasMaxLength(50)
                .HasColumnName("Assigned User ID");
            entity.Property(e => e.DimensionSetId).HasColumnName("Dimension Set ID");
            entity.Property(e => e.EntryExitPoint)
                .HasMaxLength(10)
                .HasColumnName("Entry_Exit Point");
            entity.Property(e => e.ExternalDocumentNo)
                .HasMaxLength(35)
                .HasColumnName("External Document No_");
            entity.Property(e => e.InTransitCode)
                .HasMaxLength(10)
                .HasColumnName("In-Transit Code");
            entity.Property(e => e.InboundWhseHandlingTime)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Inbound Whse_ Handling Time");
            entity.Property(e => e.LastReceiptNo)
                .HasMaxLength(20)
                .HasColumnName("Last Receipt No_");
            entity.Property(e => e.LastShipmentNo)
                .HasMaxLength(20)
                .HasColumnName("Last Shipment No_");
            entity.Property(e => e.NoSeries)
                .HasMaxLength(10)
                .HasColumnName("No_ Series");
            entity.Property(e => e.OutboundWhseHandlingTime)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Outbound Whse_ Handling Time");
            entity.Property(e => e.PostingDate)
                .HasColumnType("datetime")
                .HasColumnName("Posting Date");
            entity.Property(e => e.PostingFromWhseRef).HasColumnName("Posting from Whse_ Ref_");
            entity.Property(e => e.ReceiptDate)
                .HasColumnType("datetime")
                .HasColumnName("Receipt Date");
            entity.Property(e => e.ShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Shipment Date");
            entity.Property(e => e.ShipmentMethodCode)
                .HasMaxLength(10)
                .HasColumnName("Shipment Method Code");
            entity.Property(e => e.ShippingAdvice).HasColumnName("Shipping Advice");
            entity.Property(e => e.ShippingAgentCode)
                .HasMaxLength(10)
                .HasColumnName("Shipping Agent Code");
            entity.Property(e => e.ShippingAgentServiceCode)
                .HasMaxLength(10)
                .HasColumnName("Shipping Agent Service Code");
            entity.Property(e => e.ShippingTime)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Shipping Time");
            entity.Property(e => e.ShortcutDimension1Code)
                .HasMaxLength(20)
                .HasColumnName("Shortcut Dimension 1 Code");
            entity.Property(e => e.ShortcutDimension2Code)
                .HasMaxLength(20)
                .HasColumnName("Shortcut Dimension 2 Code");
            entity.Property(e => e.SourceDocumentNo)
                .HasMaxLength(20)
                .HasColumnName("Source Document No_");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TransactionSpecification)
                .HasMaxLength(10)
                .HasColumnName("Transaction Specification");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(10)
                .HasColumnName("Transaction Type");
            entity.Property(e => e.TransferFromAddress)
                .HasMaxLength(50)
                .HasColumnName("Transfer-from Address");
            entity.Property(e => e.TransferFromAddress2)
                .HasMaxLength(50)
                .HasColumnName("Transfer-from Address 2");
            entity.Property(e => e.TransferFromCity)
                .HasMaxLength(30)
                .HasColumnName("Transfer-from City");
            entity.Property(e => e.TransferFromCode)
                .HasMaxLength(10)
                .HasColumnName("Transfer-from Code");
            entity.Property(e => e.TransferFromContact)
                .HasMaxLength(50)
                .HasColumnName("Transfer-from Contact");
            entity.Property(e => e.TransferFromCounty)
                .HasMaxLength(30)
                .HasColumnName("Transfer-from County");
            entity.Property(e => e.TransferFromName)
                .HasMaxLength(50)
                .HasColumnName("Transfer-from Name");
            entity.Property(e => e.TransferFromName2)
                .HasMaxLength(50)
                .HasColumnName("Transfer-from Name 2");
            entity.Property(e => e.TransferFromPostCode)
                .HasMaxLength(20)
                .HasColumnName("Transfer-from Post Code");
            entity.Property(e => e.TransferToAddress)
                .HasMaxLength(50)
                .HasColumnName("Transfer-to Address");
            entity.Property(e => e.TransferToAddress2)
                .HasMaxLength(50)
                .HasColumnName("Transfer-to Address 2");
            entity.Property(e => e.TransferToCity)
                .HasMaxLength(30)
                .HasColumnName("Transfer-to City");
            entity.Property(e => e.TransferToCode)
                .HasMaxLength(10)
                .HasColumnName("Transfer-to Code");
            entity.Property(e => e.TransferToContact)
                .HasMaxLength(50)
                .HasColumnName("Transfer-to Contact");
            entity.Property(e => e.TransferToCounty)
                .HasMaxLength(30)
                .HasColumnName("Transfer-to County");
            entity.Property(e => e.TransferToName)
                .HasMaxLength(50)
                .HasColumnName("Transfer-to Name");
            entity.Property(e => e.TransferToName2)
                .HasMaxLength(50)
                .HasColumnName("Transfer-to Name 2");
            entity.Property(e => e.TransferToPostCode)
                .HasMaxLength(20)
                .HasColumnName("Transfer-to Post Code");
            entity.Property(e => e.TransportMethod)
                .HasMaxLength(10)
                .HasColumnName("Transport Method");
            entity.Property(e => e.TrsfFromCountryRegionCode)
                .HasMaxLength(10)
                .HasColumnName("Trsf_-from Country_Region Code");
            entity.Property(e => e.TrsfToCountryRegionCode)
                .HasMaxLength(10)
                .HasColumnName("Trsf_-to Country_Region Code");
        });

        modelBuilder.Entity<DcxMsWarehouseActivityHeader>(entity =>
        {
            entity.HasKey(e => new { e.Type, e.No }).HasName("DCX-MS$Warehouse Activity Header$0");

            entity.ToTable("DCX-MS$Warehouse Activity Header");

            entity.HasIndex(e => new { e.LocationCode, e.Type, e.No }, "$1").IsUnique();

            entity.HasIndex(e => new { e.SourceDocument, e.SourceNo, e.LocationCode, e.Type, e.No }, "$2").IsUnique();

            entity.Property(e => e.No)
                .HasMaxLength(20)
                .HasColumnName("No_");
            entity.Property(e => e.AssignedUserId)
                .HasMaxLength(50)
                .HasColumnName("Assigned User ID");
            entity.Property(e => e.AssignmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Assignment Date");
            entity.Property(e => e.AssignmentTime)
                .HasColumnType("datetime")
                .HasColumnName("Assignment Time");
            entity.Property(e => e.BreakbulkFilter).HasColumnName("Breakbulk Filter");
            entity.Property(e => e.DateOfLastPrinting)
                .HasColumnType("datetime")
                .HasColumnName("Date of Last Printing");
            entity.Property(e => e.DestinationNo)
                .HasMaxLength(20)
                .HasColumnName("Destination No_");
            entity.Property(e => e.DestinationType).HasColumnName("Destination Type");
            entity.Property(e => e.ExpectedReceiptDate)
                .HasColumnType("datetime")
                .HasColumnName("Expected Receipt Date");
            entity.Property(e => e.ExternalDocumentNo)
                .HasMaxLength(35)
                .HasColumnName("External Document No_");
            entity.Property(e => e.ExternalDocumentNo2)
                .HasMaxLength(35)
                .HasColumnName("External Document No_2");
            entity.Property(e => e.LastRegisteringNo)
                .HasMaxLength(20)
                .HasColumnName("Last Registering No_");
            entity.Property(e => e.LocationCode)
                .HasMaxLength(10)
                .HasColumnName("Location Code");
            entity.Property(e => e.NoPrinted).HasColumnName("No_ Printed");
            entity.Property(e => e.NoSeries)
                .HasMaxLength(10)
                .HasColumnName("No_ Series");
            entity.Property(e => e.PostingDate)
                .HasColumnType("datetime")
                .HasColumnName("Posting Date");
            entity.Property(e => e.RegisteringNo)
                .HasMaxLength(20)
                .HasColumnName("Registering No_");
            entity.Property(e => e.RegisteringNoSeries)
                .HasMaxLength(10)
                .HasColumnName("Registering No_ Series");
            entity.Property(e => e.ShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Shipment Date");
            entity.Property(e => e.SortingMethod).HasColumnName("Sorting Method");
            entity.Property(e => e.SourceDocument).HasColumnName("Source Document");
            entity.Property(e => e.SourceNo)
                .HasMaxLength(20)
                .HasColumnName("Source No_");
            entity.Property(e => e.SourceSubtype).HasColumnName("Source Subtype");
            entity.Property(e => e.SourceType).HasColumnName("Source Type");
            entity.Property(e => e.TimeOfLastPrinting)
                .HasColumnType("datetime")
                .HasColumnName("Time of Last Printing");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
        });

        modelBuilder.Entity<DcxMsWarehouseActivityLine>(entity =>
        {
            entity.HasKey(e => new { e.ActivityType, e.No, e.LineNo }).HasName("DCX-MS$Warehouse Activity Line$0");

            entity.ToTable("DCX-MS$Warehouse Activity Line");

            entity.HasIndex(e => new { e.No, e.LineNo, e.ActivityType }, "$1").IsUnique();

            entity.HasIndex(e => new { e.WhseDocumentNo, e.WhseDocumentType, e.ActivityType, e.WhseDocumentLineNo, e.ActionType, e.UnitOfMeasureCode, e.OriginalBreakbulk, e.BreakbulkNo, e.LotNo, e.SerialNo, e.AssembleToOrder, e.No, e.LineNo }, "$13").IsUnique();

            entity.HasIndex(e => new { e.ItemNo, e.BinCode, e.LocationCode, e.ActionType, e.VariantCode, e.UnitOfMeasureCode, e.BreakbulkNo, e.ActivityType, e.LotNo, e.SerialNo, e.OriginalBreakbulk, e.AssembleToOrder, e.AtoComponent, e.No, e.LineNo }, "$14").IsUnique();

            entity.HasIndex(e => new { e.ItemNo, e.LocationCode, e.ActivityType, e.BinTypeCode, e.UnitOfMeasureCode, e.VariantCode, e.BreakbulkNo, e.ActionType, e.LotNo, e.SerialNo, e.AssembleToOrder, e.No, e.LineNo }, "$15").IsUnique();

            entity.HasIndex(e => new { e.LocationCode, e.ActivityType, e.No, e.LineNo }, "$17").IsUnique();

            entity.HasIndex(e => new { e.BinCode, e.LotNo, e.ActivityType, e.No, e.LineNo }, "$19").IsUnique();

            entity.HasIndex(e => new { e.SourceType, e.SourceSubtype, e.SourceNo, e.SourceLineNo, e.SourceSublineNo, e.UnitOfMeasureCode, e.ActionType, e.BreakbulkNo, e.OriginalBreakbulk, e.ActivityType, e.AssembleToOrder, e.No, e.LineNo }, "$2").IsUnique();

            entity.Property(e => e.ActivityType).HasColumnName("Activity Type");
            entity.Property(e => e.No)
                .HasMaxLength(20)
                .HasColumnName("No_");
            entity.Property(e => e.LineNo).HasColumnName("Line No_");
            entity.Property(e => e.ActionType).HasColumnName("Action Type");
            entity.Property(e => e.AssembleToOrder).HasColumnName("Assemble to Order");
            entity.Property(e => e.AtoComponent).HasColumnName("ATO Component");
            entity.Property(e => e.BinCode)
                .HasMaxLength(20)
                .HasColumnName("Bin Code");
            entity.Property(e => e.BinRanking).HasColumnName("Bin Ranking");
            entity.Property(e => e.BinTypeCode)
                .HasMaxLength(10)
                .HasColumnName("Bin Type Code");
            entity.Property(e => e.BreakbulkNo).HasColumnName("Breakbulk No_");
            entity.Property(e => e.CrossDockInformation).HasColumnName("Cross-Dock Information");
            entity.Property(e => e.Cubage).HasColumnType("decimal(38, 20)");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Description2)
                .HasMaxLength(50)
                .HasColumnName("Description 2");
            entity.Property(e => e.DestinationNo)
                .HasMaxLength(20)
                .HasColumnName("Destination No_");
            entity.Property(e => e.DestinationType).HasColumnName("Destination Type");
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("Due Date");
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("Expiration Date");
            entity.Property(e => e.ItemNo)
                .HasMaxLength(20)
                .HasColumnName("Item No_");
            entity.Property(e => e.LocationCode)
                .HasMaxLength(10)
                .HasColumnName("Location Code");
            entity.Property(e => e.LotNo)
                .HasMaxLength(20)
                .HasColumnName("Lot No_");
            entity.Property(e => e.OriginalBreakbulk).HasColumnName("Original Breakbulk");
            entity.Property(e => e.QtyBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ (Base)");
            entity.Property(e => e.QtyHandled)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Handled");
            entity.Property(e => e.QtyHandledBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Handled (Base)");
            entity.Property(e => e.QtyOutstanding)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Outstanding");
            entity.Property(e => e.QtyOutstandingBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Outstanding (Base)");
            entity.Property(e => e.QtyPerUnitOfMeasure)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ per Unit of Measure");
            entity.Property(e => e.QtyToHandle)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Handle");
            entity.Property(e => e.QtyToHandleBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Handle (Base)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(38, 20)");
            entity.Property(e => e.SerialNo)
                .HasMaxLength(20)
                .HasColumnName("Serial No_");
            entity.Property(e => e.ShelfNo)
                .HasMaxLength(10)
                .HasColumnName("Shelf No_");
            entity.Property(e => e.ShipmentMethodCode)
                .HasMaxLength(10)
                .HasColumnName("Shipment Method Code");
            entity.Property(e => e.ShippingAdvice).HasColumnName("Shipping Advice");
            entity.Property(e => e.ShippingAgentCode)
                .HasMaxLength(10)
                .HasColumnName("Shipping Agent Code");
            entity.Property(e => e.ShippingAgentServiceCode)
                .HasMaxLength(10)
                .HasColumnName("Shipping Agent Service Code");
            entity.Property(e => e.ShippingId)
                .HasMaxLength(30)
                .HasColumnName("Shipping ID");
            entity.Property(e => e.SortingSequenceNo).HasColumnName("Sorting Sequence No_");
            entity.Property(e => e.SourceDocument).HasColumnName("Source Document");
            entity.Property(e => e.SourceLineNo).HasColumnName("Source Line No_");
            entity.Property(e => e.SourceNo)
                .HasMaxLength(20)
                .HasColumnName("Source No_");
            entity.Property(e => e.SourceSublineNo).HasColumnName("Source Subline No_");
            entity.Property(e => e.SourceSubtype).HasColumnName("Source Subtype");
            entity.Property(e => e.SourceType).HasColumnName("Source Type");
            entity.Property(e => e.SpecialEquipmentCode)
                .HasMaxLength(10)
                .HasColumnName("Special Equipment Code");
            entity.Property(e => e.StartingDate)
                .HasColumnType("datetime")
                .HasColumnName("Starting Date");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.UnitOfMeasureCode)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure Code");
            entity.Property(e => e.VariantCode)
                .HasMaxLength(10)
                .HasColumnName("Variant Code");
            entity.Property(e => e.WarrantyDate)
                .HasColumnType("datetime")
                .HasColumnName("Warranty Date");
            entity.Property(e => e.Weight).HasColumnType("decimal(38, 20)");
            entity.Property(e => e.WhseDocumentLineNo).HasColumnName("Whse_ Document Line No_");
            entity.Property(e => e.WhseDocumentNo)
                .HasMaxLength(20)
                .HasColumnName("Whse_ Document No_");
            entity.Property(e => e.WhseDocumentType).HasColumnName("Whse_ Document Type");
            entity.Property(e => e.ZoneCode)
                .HasMaxLength(10)
                .HasColumnName("Zone Code");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
