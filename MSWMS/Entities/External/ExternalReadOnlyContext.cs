using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MSWMS.Models;
using MSWMS.TempModels;

namespace MSWMS.Entities.External;

public partial class ExternalReadOnlyContext : DbContext
{
    public ExternalReadOnlyContext()
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public ExternalReadOnlyContext(DbContextOptions<ExternalReadOnlyContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    
    public override int SaveChanges()
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    public virtual DbSet<MikesportCoSALSalesHeader> MikesportCoSALSalesHeader { get; set; }

    public virtual DbSet<MikesportCoSALSalesLine> MikesportCoSALSalesLine { get; set; }

    public virtual DbSet<MikesportCoSALSalesShipmentHeader> MikesportCoSALSalesShipmentHeader { get; set; }

    public virtual DbSet<MikesportCoSALSalesShipmentLine> MikesportCoSALSalesShipmentLine { get; set; }

    public virtual DbSet<MikesportCoSALTransferHeader> MikesportCoSALTransferHeader { get; set; }

    public virtual DbSet<MikesportCoSALTransferLine> MikesportCoSALTransferLine { get; set; }

    public virtual DbSet<MikesportCoSALTransferShipmentHeader> MikesportCoSALTransferShipmentHeader { get; set; }

    public virtual DbSet<MikesportCoSALTransferShipmentLine> MikesportCoSALTransferShipmentLine { get; set; }
    
    public virtual DbSet<MikesportCoSALSalesPrice> MikesportCoSALSalesPrices { get; set; }
    
    public virtual DbSet<MikesportCoSALDefaultSalesPrice> MikesportCoSALDefaultSalesPrices { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_100_CS_AS");

        modelBuilder.Entity<MikesportCoSALSalesHeader>(entity =>
        {
            entity.HasKey(e => new { e.DocumentType, e.No }).HasName("Mikesport & Co_ S_A_L_$Sales Header$0");

            entity.ToTable("Mikesport & Co_ S_A_L_$Sales Header");

            entity.HasIndex(e => new { e.No, e.DocumentType }, "$1").IsUnique();

            entity.HasIndex(e => new { e.LocationCode, e.OrderDate, e.DocumentType, e.No }, "$10").IsUnique();

            entity.HasIndex(e => new { e.ShipToPostCode, e.DocumentType, e.No }, "$11").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.SellToCustomerNo, e.SellToContactNo, e.No }, "$12").IsUnique();

            entity.HasIndex(e => new { e.OwnershipSendDate, e.DocumentType, e.No }, "$13").IsUnique();

            entity.HasIndex(e => new { e.ReplicationCounter, e.DocumentType, e.No }, "$14").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.SellToCustomerNo, e.No }, "$2").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.BillToCustomerNo, e.No }, "$3").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.CombineShipments, e.BillToCustomerNo, e.CurrencyCode, e.Eu3PartyTrade, e.No }, "$4").IsUnique();

            entity.HasIndex(e => new { e.SellToCustomerNo, e.ExternalDocumentNo, e.DocumentType, e.No }, "$5").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.SellToContactNo, e.No }, "$6").IsUnique();

            entity.HasIndex(e => new { e.BillToContactNo, e.DocumentType, e.No }, "$7").IsUnique();

            entity.HasIndex(e => new { e.IncomingDocumentEntryNo, e.DocumentType, e.No }, "$8").IsUnique();

            entity.HasIndex(e => new { e.SalespersonCode, e.OrderDate, e.DocumentType, e.No }, "$9").IsUnique();

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
            entity.Property(e => e.BatchNo)
                .HasMaxLength(10)
                .HasColumnName("Batch No_");
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
            entity.Property(e => e.BillToHouseNo)
                .HasMaxLength(30)
                .HasColumnName("Bill-to House No_");
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
            entity.Property(e => e.CashierId)
                .HasMaxLength(10)
                .HasColumnName("Cashier ID");
            entity.Property(e => e.CombineShipments).HasColumnName("Combine Shipments");
            entity.Property(e => e.CompressPrepayment).HasColumnName("Compress Prepayment");
            entity.Property(e => e.ContEMail).HasColumnName("Cont_ E-mail");
            entity.Property(e => e.ContMail).HasColumnName("Cont_ Mail");
            entity.Property(e => e.ContPhone).HasColumnName("Cont_ Phone");
            entity.Property(e => e.CreatedByUser)
                .HasMaxLength(50)
                .HasColumnName("Created By User");
            entity.Property(e => e.CreditCardNo)
                .HasMaxLength(20)
                .HasColumnName("Credit Card No_");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasColumnName("Currency Code");
            entity.Property(e => e.CurrencyFactor)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Currency Factor");
            entity.Property(e => e.CustomerContactDate)
                .HasColumnType("datetime")
                .HasColumnName("Customer Contact Date");
            entity.Property(e => e.CustomerDiscGroup)
                .HasMaxLength(20)
                .HasColumnName("Customer Disc_ Group");
            entity.Property(e => e.CustomerPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Customer Posting Group");
            entity.Property(e => e.CustomerPriceGroup)
                .HasMaxLength(10)
                .HasColumnName("Customer Price Group");
            entity.Property(e => e.DateTimeCreated)
                .HasColumnType("datetime")
                .HasColumnName("Date _ Time Created");
            entity.Property(e => e.DaytimePhoneNo)
                .HasMaxLength(30)
                .HasColumnName("Daytime Phone No_");
            entity.Property(e => e.DeliveryReferenceNo)
                .HasMaxLength(30)
                .HasColumnName("Delivery Reference No");
            entity.Property(e => e.DimensionSetId).HasColumnName("Dimension Set ID");
            entity.Property(e => e.DirectDebitMandateId)
                .HasMaxLength(35)
                .HasColumnName("Direct Debit Mandate ID");
            entity.Property(e => e.DocNoOccurrence).HasColumnName("Doc_ No_ Occurrence");
            entity.Property(e => e.DocumentDate)
                .HasColumnType("datetime")
                .HasColumnName("Document Date");
            entity.Property(e => e.DocumentsSpecialVatRate)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Documents Special VAT Rate");
            entity.Property(e => e.DocumentsStandardVatRate)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Documents Standard VAT Rate");
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("Due Date");
            entity.Property(e => e.EMail)
                .HasMaxLength(80)
                .HasColumnName("E-Mail");
            entity.Property(e => e.EstimatedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Estimated Delivery Date");
            entity.Property(e => e.Eu3PartyTrade).HasColumnName("EU 3-Party Trade");
            entity.Property(e => e.ExchRateTypeCurrFactor)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Exch_ Rate Type Curr_ Factor");
            entity.Property(e => e.ExchangeRateTypeCode)
                .HasMaxLength(20)
                .HasColumnName("Exchange Rate Type Code");
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
            entity.Property(e => e.InStoreCreatedEntry).HasColumnName("InStore-Created Entry");
            entity.Property(e => e.IncomingDocumentEntryNo).HasColumnName("Incoming Document Entry No_");
            entity.Property(e => e.IntegrationStatus).HasColumnName("Integration Status");
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
            entity.Property(e => e.LastDateTimeModified)
                .HasColumnType("datetime")
                .HasColumnName("Last Date _ Time Modified");
            entity.Property(e => e.LastModifiedByUser)
                .HasMaxLength(50)
                .HasColumnName("Last Modified by User");
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
            entity.Property(e => e.MemberCardNo)
                .HasMaxLength(100)
                .HasColumnName("Member Card No_");
            entity.Property(e => e.MobilePhoneNo)
                .HasMaxLength(30)
                .HasColumnName("Mobile Phone No_");
            entity.Property(e => e.NoPrinted).HasColumnName("No_ Printed");
            entity.Property(e => e.NoSeries)
                .HasMaxLength(10)
                .HasColumnName("No_ Series");
            entity.Property(e => e.NotShowDialog).HasColumnName("Not Show Dialog");
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
            entity.Property(e => e.OwnershipSendDate)
                .HasColumnType("datetime")
                .HasColumnName("Ownership Send Date");
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
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(30)
                .HasColumnName("Phone No_");
            entity.Property(e => e.PmtDiscountDate)
                .HasColumnType("datetime")
                .HasColumnName("Pmt_ Discount Date");
            entity.Property(e => e.PosComment)
                .HasMaxLength(30)
                .HasColumnName("POS Comment");
            entity.Property(e => e.PosId)
                .HasMaxLength(20)
                .HasColumnName("POS ID");
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
            entity.Property(e => e.PostingTime)
                .HasColumnType("datetime")
                .HasColumnName("Posting Time");
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
            entity.Property(e => e.ReceivingPickingNo)
                .HasMaxLength(20)
                .HasColumnName("Receiving_Picking No_");
            entity.Property(e => e.ReplicationCounter).HasColumnName("Replication Counter");
            entity.Property(e => e.RequestedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Requested Delivery Date");
            entity.Property(e => e.ResponsibilityCenter)
                .HasMaxLength(10)
                .HasColumnName("Responsibility Center");
            entity.Property(e => e.RetailSpecialOrder).HasColumnName("Retail Special Order");
            entity.Property(e => e.RetailStatus).HasColumnName("Retail Status");
            entity.Property(e => e.RetailZonesCode)
                .HasMaxLength(10)
                .HasColumnName("Retail Zones Code");
            entity.Property(e => e.ReturnReceiptNo)
                .HasMaxLength(20)
                .HasColumnName("Return Receipt No_");
            entity.Property(e => e.ReturnReceiptNoSeries)
                .HasMaxLength(10)
                .HasColumnName("Return Receipt No_ Series");
            entity.Property(e => e.RingBackDate)
                .HasColumnType("datetime")
                .HasColumnName("Ring Back Date");
            entity.Property(e => e.SalesType)
                .HasMaxLength(20)
                .HasColumnName("Sales Type");
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
            entity.Property(e => e.SendToWms).HasColumnName("Send To WMS");
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
            entity.Property(e => e.ShipToHouseNo)
                .HasMaxLength(30)
                .HasColumnName("Ship-to House No_");
            entity.Property(e => e.ShipToName)
                .HasMaxLength(50)
                .HasColumnName("Ship-to Name");
            entity.Property(e => e.ShipToName2)
                .HasMaxLength(50)
                .HasColumnName("Ship-to Name 2");
            entity.Property(e => e.ShipToPostCode)
                .HasMaxLength(20)
                .HasColumnName("Ship-to Post Code");
            entity.Property(e => e.ShipToTelephone)
                .HasMaxLength(30)
                .HasColumnName("Ship-To Telephone");
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
            entity.Property(e => e.SourceCode)
                .HasMaxLength(10)
                .HasColumnName("Source Code");
            entity.Property(e => e.SpecialOrderOrigin).HasColumnName("Special Order Origin");
            entity.Property(e => e.SpecialVatRateEnabled).HasColumnName("Special VAT Rate Enabled");
            entity.Property(e => e.SpoCreatedEntry).HasColumnName("SPO-Created Entry");
            entity.Property(e => e.StatementNo)
                .HasMaxLength(20)
                .HasColumnName("Statement No_");
            entity.Property(e => e.StoreCommission)
                .HasMaxLength(10)
                .HasColumnName("Store Commission");
            entity.Property(e => e.StoreNo)
                .HasMaxLength(10)
                .HasColumnName("Store No_");
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
            entity.Property(e => e.ValidityDate)
                .HasColumnType("datetime")
                .HasColumnName("Validity Date");
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
            entity.Property(e => e.XRetailLocationCode)
                .HasMaxLength(10)
                .HasColumnName("xRetail Location Code");
            entity.Property(e => e.YourReference)
                .HasMaxLength(35)
                .HasColumnName("Your Reference");
        });

        modelBuilder.Entity<MikesportCoSALSalesLine>(entity =>
        {
            entity.HasKey(e => new { e.DocumentType, e.DocumentNo, e.LineNo }).HasName("Mikesport & Co_ S_A_L_$Sales Line$0");

            entity.ToTable("Mikesport & Co_ S_A_L_$Sales Line");

            entity.HasIndex(e => new { e.DocumentNo, e.LineNo, e.DocumentType }, "$1").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.SellToCustomerNo, e.ShipmentNo, e.DocumentNo, e.LineNo }, "$10").IsUnique();

            entity.HasIndex(e => new { e.JobContractEntryNo, e.DocumentType, e.DocumentNo, e.LineNo }, "$11").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.DocumentNo, e.QtyShippedNotInvoiced, e.LineNo }, "$12").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.DocumentNo, e.Type, e.No, e.LineNo }, "$13").IsUnique();

            entity.HasIndex(e => new { e.RetailSpecialOrder, e.DocumentType, e.DocumentNo, e.LineNo }, "$14").IsUnique();

            entity.HasIndex(e => new { e.RetailSpecialOrder, e.SourcingStatus, e.LocationCode, e.SpoDocumentMethod, e.DocumentType, e.DocumentNo, e.LineNo }, "$15").IsUnique();

            entity.HasIndex(e => new { e.RetailSpecialOrder, e.DeliveryLocationCode, e.DeliveryStatus, e.DocumentType, e.DocumentNo, e.LineNo }, "$16").IsUnique();

            entity.HasIndex(e => new { e.RetailSpecialOrder, e.SpoWhseLocation, e.WhseProcess, e.DocumentType, e.DocumentNo, e.LineNo }, "$17").IsUnique();

            entity.HasIndex(e => new { e.AllocPlanPurcOrderNo, e.DocumentType, e.Type, e.No, e.VariantCode, e.DocumentNo, e.LineNo }, "$18").IsUnique();

            entity.HasIndex(e => new { e.Division, e.ItemCategoryCode, e.ProductGroupCode, e.ItemFamilyCode, e.ItemBrandCode, e.PriceRangeCode, e.ItemGenderLevel1Code, e.ItemGenderLevel2Code, e.SeasonCode, e.ItemLineCode, e.IsConsigned, e.DocumentType, e.DocumentNo, e.LineNo }, "$19").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.Type, e.No, e.VariantCode, e.DropShipment, e.LocationCode, e.ShipmentDate, e.DocumentNo, e.LineNo }, "$2").IsUnique();

            entity.HasIndex(e => new { e.ReplicationCounter, e.DocumentType, e.DocumentNo, e.LineNo }, "$20").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.BillToCustomerNo, e.CurrencyCode, e.DocumentNo, e.LineNo }, "$3").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.BlanketOrderNo, e.BlanketOrderLineNo, e.DocumentNo, e.LineNo }, "$6").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.DocumentNo, e.LocationCode, e.LineNo }, "$7").IsUnique();

            entity.HasIndex(e => new { e.DocumentType, e.ShipmentNo, e.ShipmentLineNo, e.DocumentNo, e.LineNo }, "$8").IsUnique();

            entity.Property(e => e.DocumentType).HasColumnName("Document Type");
            entity.Property(e => e.DocumentNo)
                .HasMaxLength(20)
                .HasColumnName("Document No_");
            entity.Property(e => e.LineNo).HasColumnName("Line No_");
            entity.Property(e => e.AddChargeCode)
                .HasMaxLength(10)
                .HasColumnName("Add_charge code");
            entity.Property(e => e.AddChargeOption).HasColumnName("Add_Charge Option");
            entity.Property(e => e.AllocPlanPurcOrderNo)
                .HasMaxLength(20)
                .HasColumnName("Alloc_ Plan Purc_ Order No_");
            entity.Property(e => e.AllowInvoiceDisc).HasColumnName("Allow Invoice Disc_");
            entity.Property(e => e.AllowItemChargeAssignment).HasColumnName("Allow Item Charge Assignment");
            entity.Property(e => e.AllowLineDisc).HasColumnName("Allow Line Disc_");
            entity.Property(e => e.Amount).HasColumnType("decimal(38, 20)");
            entity.Property(e => e.AmountIncludingVat)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Amount Including VAT");
            entity.Property(e => e.AmountIncludingVatLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Amount Including VAT (LCY)");
            entity.Property(e => e.ApplFromItemEntry).HasColumnName("Appl_-from Item Entry");
            entity.Property(e => e.ApplToItemEntry).HasColumnName("Appl_-to Item Entry");
            entity.Property(e => e.Area).HasMaxLength(10);
            entity.Property(e => e.AttachedToLineNo).HasColumnName("Attached to Line No_");
            entity.Property(e => e.AutoAccGroup)
                .HasMaxLength(10)
                .HasColumnName("Auto_ Acc_ Group");
            entity.Property(e => e.BarcodeNo)
                .HasMaxLength(20)
                .HasColumnName("Barcode No_");
            entity.Property(e => e.BillToCustomerNo)
                .HasMaxLength(20)
                .HasColumnName("Bill-to Customer No_");
            entity.Property(e => e.BinCode)
                .HasMaxLength(20)
                .HasColumnName("Bin Code");
            entity.Property(e => e.BlanketOrderLineNo).HasColumnName("Blanket Order Line No_");
            entity.Property(e => e.BlanketOrderNo)
                .HasMaxLength(20)
                .HasColumnName("Blanket Order No_");
            entity.Property(e => e.BomItemNo)
                .HasMaxLength(20)
                .HasColumnName("BOM Item No_");
            entity.Property(e => e.CancelPermitted).HasColumnName("Cancel Permitted");
            entity.Property(e => e.CompletelyShipped).HasColumnName("Completely Shipped");
            entity.Property(e => e.ConfigurationId)
                .HasMaxLength(30)
                .HasColumnName("Configuration ID");
            entity.Property(e => e.Counter).HasColumnType("decimal(38, 20)");
            entity.Property(e => e.CreatedAtPosTermNo)
                .HasMaxLength(10)
                .HasColumnName("Created At POS Term No_");
            entity.Property(e => e.CrossReferenceNo)
                .HasMaxLength(20)
                .HasColumnName("Cross-Reference No_");
            entity.Property(e => e.CrossReferenceType).HasColumnName("Cross-Reference Type");
            entity.Property(e => e.CrossReferenceTypeNo)
                .HasMaxLength(30)
                .HasColumnName("Cross-Reference Type No_");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasColumnName("Currency Code");
            entity.Property(e => e.CurrentCustPriceGroup)
                .HasMaxLength(20)
                .HasColumnName("Current Cust_ Price Group");
            entity.Property(e => e.CurrentStoreGroup)
                .HasMaxLength(20)
                .HasColumnName("Current Store Group");
            entity.Property(e => e.CustomerDiscGroup)
                .HasMaxLength(20)
                .HasColumnName("Customer Disc_ Group");
            entity.Property(e => e.CustomerPriceGroup)
                .HasMaxLength(10)
                .HasColumnName("Customer Price Group");
            entity.Property(e => e.DeliverFrom).HasColumnName("Deliver from");
            entity.Property(e => e.DeliveringMethod).HasColumnName("Delivering Method");
            entity.Property(e => e.DeliveryCancelDateTime)
                .HasColumnType("datetime")
                .HasColumnName("Delivery_Cancel Date Time");
            entity.Property(e => e.DeliveryCancelUserId)
                .HasMaxLength(50)
                .HasColumnName("Delivery_Cancel User ID");
            entity.Property(e => e.DeliveryLocationCode)
                .HasMaxLength(10)
                .HasColumnName("Delivery Location Code");
            entity.Property(e => e.DeliveryReferenceNo)
                .HasMaxLength(30)
                .HasColumnName("Delivery Reference No");
            entity.Property(e => e.DeliveryStatus).HasColumnName("Delivery Status");
            entity.Property(e => e.DeprUntilFaPostingDate).HasColumnName("Depr_ until FA Posting Date");
            entity.Property(e => e.DepreciationBookCode)
                .HasMaxLength(10)
                .HasColumnName("Depreciation Book Code");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Description2)
                .HasMaxLength(50)
                .HasColumnName("Description 2");
            entity.Property(e => e.DimensionSetId).HasColumnName("Dimension Set ID");
            entity.Property(e => e.Division).HasMaxLength(10);
            entity.Property(e => e.DropShipment).HasColumnName("Drop Shipment");
            entity.Property(e => e.DuplicateInDepreciationBook)
                .HasMaxLength(10)
                .HasColumnName("Duplicate in Depreciation Book");
            entity.Property(e => e.ErrorInProcess).HasColumnName("Error in Process");
            entity.Property(e => e.EstimatedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Estimated Delivery Date");
            entity.Property(e => e.ExcludeZeroPricePrinting).HasColumnName("Exclude Zero Price Printing");
            entity.Property(e => e.ExitPoint)
                .HasMaxLength(10)
                .HasColumnName("Exit Point");
            entity.Property(e => e.FaPostingDate)
                .HasColumnType("datetime")
                .HasColumnName("FA Posting Date");
            entity.Property(e => e.GenBusPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Gen_ Bus_ Posting Group");
            entity.Property(e => e.GenProdPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Gen_ Prod_ Posting Group");
            entity.Property(e => e.GrossWeight)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Gross Weight");
            entity.Property(e => e.IcPartnerCode)
                .HasMaxLength(20)
                .HasColumnName("IC Partner Code");
            entity.Property(e => e.IcPartnerRefType).HasColumnName("IC Partner Ref_ Type");
            entity.Property(e => e.IcPartnerReference)
                .HasMaxLength(20)
                .HasColumnName("IC Partner Reference");
            entity.Property(e => e.InvDiscAmountToInvoice)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Inv_ Disc_ Amount to Invoice");
            entity.Property(e => e.InvDiscountAmount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Inv_ Discount Amount");
            entity.Property(e => e.IsConsigned).HasColumnName("Is Consigned");
            entity.Property(e => e.ItemBrandCode)
                .HasMaxLength(20)
                .HasColumnName("Item Brand Code");
            entity.Property(e => e.ItemCategoryCode)
                .HasMaxLength(10)
                .HasColumnName("Item Category Code");
            entity.Property(e => e.ItemFamilyCode)
                .HasMaxLength(10)
                .HasColumnName("Item Family Code");
            entity.Property(e => e.ItemGenderLevel1Code)
                .HasMaxLength(20)
                .HasColumnName("Item Gender Level 1 Code");
            entity.Property(e => e.ItemGenderLevel2Code)
                .HasMaxLength(20)
                .HasColumnName("Item Gender Level 2 Code");
            entity.Property(e => e.ItemLineCode)
                .HasMaxLength(20)
                .HasColumnName("Item Line Code");
            entity.Property(e => e.ItemTrackingNo)
                .HasMaxLength(20)
                .HasColumnName("Item Tracking No_");
            entity.Property(e => e.JobContractEntryNo).HasColumnName("Job Contract Entry No_");
            entity.Property(e => e.JobNo)
                .HasMaxLength(20)
                .HasColumnName("Job No_");
            entity.Property(e => e.JobTaskNo)
                .HasMaxLength(20)
                .HasColumnName("Job Task No_");
            entity.Property(e => e.LineAmount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Line Amount");
            entity.Property(e => e.LineDiscount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Line Discount _");
            entity.Property(e => e.LineDiscountAmount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Line Discount Amount");
            entity.Property(e => e.LinkedToLineNo).HasColumnName("Linked to Line No_");
            entity.Property(e => e.LocationCode)
                .HasMaxLength(10)
                .HasColumnName("Location Code");
            entity.Property(e => e.NetWeight)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Net Weight");
            entity.Property(e => e.No)
                .HasMaxLength(20)
                .HasColumnName("No_");
            entity.Property(e => e.NoLaterThanDate)
                .HasColumnType("datetime")
                .HasColumnName("No later than Date");
            entity.Property(e => e.NonRefundAmount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Non Refund Amount");
            entity.Property(e => e.OfferNo)
                .HasMaxLength(20)
                .HasColumnName("Offer No_");
            entity.Property(e => e.OptionValueText)
                .HasMaxLength(100)
                .HasColumnName("Option Value Text");
            entity.Property(e => e.OriginallyOrderedNo)
                .HasMaxLength(20)
                .HasColumnName("Originally Ordered No_");
            entity.Property(e => e.OriginallyOrderedVarCode)
                .HasMaxLength(10)
                .HasColumnName("Originally Ordered Var_ Code");
            entity.Property(e => e.OutOfStockSubstitution).HasColumnName("Out-of-Stock Substitution");
            entity.Property(e => e.OutboundWhseHandlingTime)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Outbound Whse_ Handling Time");
            entity.Property(e => e.OutstandingAmount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Outstanding Amount");
            entity.Property(e => e.OutstandingAmountLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Outstanding Amount (LCY)");
            entity.Property(e => e.OutstandingQtyBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Outstanding Qty_ (Base)");
            entity.Property(e => e.OutstandingQuantity)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Outstanding Quantity");
            entity.Property(e => e.PaymentAtDeliveryLimit)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Payment-At Delivery-Limit");
            entity.Property(e => e.PaymentAtOrderEntryLimit)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Payment-At Order Entry-Limit");
            entity.Property(e => e.PaymentAtPurchaseOrderLimit)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Payment-At PurchaseOrder-Limit");
            entity.Property(e => e.PaymentProfileCode)
                .HasMaxLength(10)
                .HasColumnName("Payment Profile Code");
            entity.Property(e => e.PlannedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Planned Delivery Date");
            entity.Property(e => e.PlannedShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Planned Shipment Date");
            entity.Property(e => e.PostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Posting Group");
            entity.Property(e => e.Prepayment)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepayment _");
            entity.Property(e => e.PrepaymentAmount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepayment Amount");
            entity.Property(e => e.PrepaymentLine).HasColumnName("Prepayment Line");
            entity.Property(e => e.PrepaymentTaxAreaCode)
                .HasMaxLength(20)
                .HasColumnName("Prepayment Tax Area Code");
            entity.Property(e => e.PrepaymentTaxGroupCode)
                .HasMaxLength(10)
                .HasColumnName("Prepayment Tax Group Code");
            entity.Property(e => e.PrepaymentTaxLiable).HasColumnName("Prepayment Tax Liable");
            entity.Property(e => e.PrepaymentVat)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepayment VAT _");
            entity.Property(e => e.PrepaymentVatDifference)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepayment VAT Difference");
            entity.Property(e => e.PrepaymentVatIdentifier)
                .HasMaxLength(10)
                .HasColumnName("Prepayment VAT Identifier");
            entity.Property(e => e.PrepmtAmountInvInclVat)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt_ Amount Inv_ Incl_ VAT");
            entity.Property(e => e.PrepmtAmountInvLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt_ Amount Inv_ (LCY)");
            entity.Property(e => e.PrepmtAmtDeducted)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt Amt Deducted");
            entity.Property(e => e.PrepmtAmtInclVat)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt_ Amt_ Incl_ VAT");
            entity.Property(e => e.PrepmtAmtInv)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt_ Amt_ Inv_");
            entity.Property(e => e.PrepmtAmtToDeduct)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt Amt to Deduct");
            entity.Property(e => e.PrepmtLineAmount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt_ Line Amount");
            entity.Property(e => e.PrepmtVatAmountInvLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt_ VAT Amount Inv_ (LCY)");
            entity.Property(e => e.PrepmtVatBaseAmt)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt_ VAT Base Amt_");
            entity.Property(e => e.PrepmtVatCalcType).HasColumnName("Prepmt_ VAT Calc_ Type");
            entity.Property(e => e.PrepmtVatDiffDeducted)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt VAT Diff_ Deducted");
            entity.Property(e => e.PrepmtVatDiffToDeduct)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Prepmt VAT Diff_ to Deduct");
            entity.Property(e => e.PriceRangeCode)
                .HasMaxLength(20)
                .HasColumnName("Price Range Code");
            entity.Property(e => e.ProductGroupCode)
                .HasMaxLength(10)
                .HasColumnName("Product Group Code");
            entity.Property(e => e.Profit)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Profit _");
            entity.Property(e => e.PromisedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Promised Delivery Date");
            entity.Property(e => e.PromotionNo)
                .HasMaxLength(20)
                .HasColumnName("Promotion No_");
            entity.Property(e => e.PurchOrderLineNo).HasColumnName("Purch_ Order Line No_");
            entity.Property(e => e.PurchaseOrderNo)
                .HasMaxLength(20)
                .HasColumnName("Purchase Order No_");
            entity.Property(e => e.PurchasingCode)
                .HasMaxLength(10)
                .HasColumnName("Purchasing Code");
            entity.Property(e => e.QtyInvoicedBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Invoiced (Base)");
            entity.Property(e => e.QtyPerUnitOfMeasure)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ per Unit of Measure");
            entity.Property(e => e.QtyShippedBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Shipped (Base)");
            entity.Property(e => e.QtyShippedNotInvdBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Shipped Not Invd_ (Base)");
            entity.Property(e => e.QtyShippedNotInvoiced)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Shipped Not Invoiced");
            entity.Property(e => e.QtyToAsmToOrderBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Asm_ to Order (Base)");
            entity.Property(e => e.QtyToAssembleToOrder)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Assemble to Order");
            entity.Property(e => e.QtyToInvoice)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Invoice");
            entity.Property(e => e.QtyToInvoiceBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Invoice (Base)");
            entity.Property(e => e.QtyToShip)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Ship");
            entity.Property(e => e.QtyToShipBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Ship (Base)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(38, 20)");
            entity.Property(e => e.QuantityBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Quantity (Base)");
            entity.Property(e => e.QuantityInvoiced)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Quantity Invoiced");
            entity.Property(e => e.QuantityShipped)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Quantity Shipped");
            entity.Property(e => e.ReplicationCounter).HasColumnName("Replication Counter");
            entity.Property(e => e.RequestedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Requested Delivery Date");
            entity.Property(e => e.ReservedByPosNo)
                .HasMaxLength(10)
                .HasColumnName("Reserved By POS No_");
            entity.Property(e => e.ResponsibilityCenter)
                .HasMaxLength(10)
                .HasColumnName("Responsibility Center");
            entity.Property(e => e.RetQtyRcdNotInvdBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Ret_ Qty_ Rcd_ Not Invd_(Base)");
            entity.Property(e => e.RetailSpecialOrder).HasColumnName("Retail Special Order");
            entity.Property(e => e.ReturnPolicy).HasColumnName("Return Policy");
            entity.Property(e => e.ReturnQtyRcdNotInvd)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Return Qty_ Rcd_ Not Invd_");
            entity.Property(e => e.ReturnQtyReceived)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Return Qty_ Received");
            entity.Property(e => e.ReturnQtyReceivedBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Return Qty_ Received (Base)");
            entity.Property(e => e.ReturnQtyToReceive)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Return Qty_ to Receive");
            entity.Property(e => e.ReturnQtyToReceiveBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Return Qty_ to Receive (Base)");
            entity.Property(e => e.ReturnRcdNotInvd)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Return Rcd_ Not Invd_");
            entity.Property(e => e.ReturnRcdNotInvdLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Return Rcd_ Not Invd_ (LCY)");
            entity.Property(e => e.ReturnReasonCode)
                .HasMaxLength(10)
                .HasColumnName("Return Reason Code");
            entity.Property(e => e.ReturnReceiptLineNo).HasColumnName("Return Receipt Line No_");
            entity.Property(e => e.ReturnReceiptNo)
                .HasMaxLength(20)
                .HasColumnName("Return Receipt No_");
            entity.Property(e => e.SeasonCode)
                .HasMaxLength(10)
                .HasColumnName("Season Code");
            entity.Property(e => e.SellToCustomerNo)
                .HasMaxLength(20)
                .HasColumnName("Sell-to Customer No_");
            entity.Property(e => e.SendToWms).HasColumnName("Send To WMS");
            entity.Property(e => e.ShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Shipment Date");
            entity.Property(e => e.ShipmentLineNo).HasColumnName("Shipment Line No_");
            entity.Property(e => e.ShipmentNo)
                .HasMaxLength(20)
                .HasColumnName("Shipment No_");
            entity.Property(e => e.ShippedNotInvoiced)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Shipped Not Invoiced");
            entity.Property(e => e.ShippedNotInvoicedLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Shipped Not Invoiced (LCY)");
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
            entity.Property(e => e.SourcingStatus).HasColumnName("Sourcing Status");
            entity.Property(e => e.SpecialOrder).HasColumnName("Special Order");
            entity.Property(e => e.SpecialOrderPurchLineNo).HasColumnName("Special Order Purch_ Line No_");
            entity.Property(e => e.SpecialOrderPurchaseNo)
                .HasMaxLength(20)
                .HasColumnName("Special Order Purchase No_");
            entity.Property(e => e.SpoDocumentMethod).HasColumnName("SPO Document Method");
            entity.Property(e => e.SpoPrepayment)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("SPO Prepayment _");
            entity.Property(e => e.SpoWhseLocation)
                .HasMaxLength(10)
                .HasColumnName("SPO Whse Location");
            entity.Property(e => e.StoreNo)
                .HasMaxLength(10)
                .HasColumnName("Store No_");
            entity.Property(e => e.StoreSalesLocation)
                .HasMaxLength(10)
                .HasColumnName("Store Sales Location");
            entity.Property(e => e.SystemCreatedEntry).HasColumnName("System-Created Entry");
            entity.Property(e => e.TaxAreaCode)
                .HasMaxLength(20)
                .HasColumnName("Tax Area Code");
            entity.Property(e => e.TaxGroupCode)
                .HasMaxLength(10)
                .HasColumnName("Tax Group Code");
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
            entity.Property(e => e.UnitCost)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Cost");
            entity.Property(e => e.UnitCostLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Cost (LCY)");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure");
            entity.Property(e => e.UnitOfMeasureCode)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure Code");
            entity.Property(e => e.UnitOfMeasureCrossRef)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure (Cross Ref_)");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Price");
            entity.Property(e => e.UnitVolume)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Volume");
            entity.Property(e => e.UnitsPerParcel)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Units per Parcel");
            entity.Property(e => e.UseDuplicationList).HasColumnName("Use Duplication List");
            entity.Property(e => e.VariantCode)
                .HasMaxLength(10)
                .HasColumnName("Variant Code");
            entity.Property(e => e.Vat)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("VAT _");
            entity.Property(e => e.VatAmntInOriginalCurrency)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("VAT Amnt In Original Currency");
            entity.Property(e => e.VatAmountInLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("VAT Amount In LCY");
            entity.Property(e => e.VatBaseAmount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("VAT Base Amount");
            entity.Property(e => e.VatBusPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("VAT Bus_ Posting Group");
            entity.Property(e => e.VatCalculationType).HasColumnName("VAT Calculation Type");
            entity.Property(e => e.VatClauseCode)
                .HasMaxLength(10)
                .HasColumnName("VAT Clause Code");
            entity.Property(e => e.VatDifference)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("VAT Difference");
            entity.Property(e => e.VatIdentifier)
                .HasMaxLength(10)
                .HasColumnName("VAT Identifier");
            entity.Property(e => e.VatProdPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("VAT Prod_ Posting Group");
            entity.Property(e => e.VendorDeliversTo).HasColumnName("Vendor Delivers to");
            entity.Property(e => e.VendorNo)
                .HasMaxLength(20)
                .HasColumnName("Vendor No_");
            entity.Property(e => e.WarrantyPeriod)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Warranty Period");
            entity.Property(e => e.WhseProcess).HasColumnName("Whse Process");
            entity.Property(e => e.WmsStatus).HasColumnName("WMS Status");
            entity.Property(e => e.WorkTypeCode)
                .HasMaxLength(10)
                .HasColumnName("Work Type Code");
        });

        modelBuilder.Entity<MikesportCoSALSalesShipmentHeader>(entity =>
        {
            entity.HasMany(header => header.Lines)
                .WithOne(line => line.Header)
                .HasForeignKey(line => line.DocumentNo)
                .HasPrincipalKey(header => header.No);
            
            entity.HasKey(e => e.No).HasName("Mikesport & Co_ S_A_L_$Sales Shipment Header$0");

            entity.ToTable("Mikesport & Co_ S_A_L_$Sales Shipment Header");

            entity.HasIndex(e => new { e.OrderNo, e.No }, "$1").IsUnique();

            entity.HasIndex(e => new { e.BillToCustomerNo, e.No }, "$2").IsUnique();

            entity.HasIndex(e => new { e.SellToCustomerNo, e.ExternalDocumentNo, e.No }, "$3").IsUnique();

            entity.HasIndex(e => new { e.SellToCustomerNo, e.No }, "$4").IsUnique();

            entity.HasIndex(e => new { e.RecivingPickingNo, e.No }, "$5").IsUnique();

            entity.HasIndex(e => new { e.StatementNo, e.No }, "$6").IsUnique();

            entity.Property(e => e.No)
                .HasMaxLength(20)
                .HasColumnName("No_");
            entity.Property(e => e.AllowLineDisc).HasColumnName("Allow Line Disc_");
            entity.Property(e => e.AppliesToDocNo)
                .HasMaxLength(20)
                .HasColumnName("Applies-to Doc_ No_");
            entity.Property(e => e.AppliesToDocType).HasColumnName("Applies-to Doc_ Type");
            entity.Property(e => e.Area).HasMaxLength(10);
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
            entity.Property(e => e.BillToHouseNo)
                .HasMaxLength(30)
                .HasColumnName("Bill-to House No_");
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
            entity.Property(e => e.ContEMail).HasColumnName("Cont_ E-mail");
            entity.Property(e => e.ContMail).HasColumnName("Cont_ Mail");
            entity.Property(e => e.ContPhone).HasColumnName("Cont_ Phone");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasColumnName("Currency Code");
            entity.Property(e => e.CurrencyFactor)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Currency Factor");
            entity.Property(e => e.CustomerContactDate)
                .HasColumnType("datetime")
                .HasColumnName("Customer Contact Date");
            entity.Property(e => e.CustomerDiscGroup)
                .HasMaxLength(20)
                .HasColumnName("Customer Disc_ Group");
            entity.Property(e => e.CustomerPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Customer Posting Group");
            entity.Property(e => e.CustomerPriceGroup)
                .HasMaxLength(10)
                .HasColumnName("Customer Price Group");
            entity.Property(e => e.DaytimePhoneNo)
                .HasMaxLength(30)
                .HasColumnName("Daytime Phone No_");
            entity.Property(e => e.DeliveryReferenceNo)
                .HasMaxLength(30)
                .HasColumnName("Delivery Reference No");
            entity.Property(e => e.DimensionSetId).HasColumnName("Dimension Set ID");
            entity.Property(e => e.DocumentDate)
                .HasColumnType("datetime")
                .HasColumnName("Document Date");
            entity.Property(e => e.DocumentsSpecialVatRate)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Documents Special VAT Rate");
            entity.Property(e => e.DocumentsStandardVatRate)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Documents Standard VAT Rate");
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("Due Date");
            entity.Property(e => e.EMail)
                .HasMaxLength(80)
                .HasColumnName("E-Mail");
            entity.Property(e => e.EstimatedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Estimated Delivery Date");
            entity.Property(e => e.Eu3PartyTrade).HasColumnName("EU 3-Party Trade");
            entity.Property(e => e.ExchRateTypeCurrFactor)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Exch_ Rate Type Curr_ Factor");
            entity.Property(e => e.ExchangeRateTypeCode)
                .HasMaxLength(20)
                .HasColumnName("Exchange Rate Type Code");
            entity.Property(e => e.ExitPoint)
                .HasMaxLength(10)
                .HasColumnName("Exit Point");
            entity.Property(e => e.ExternalDocumentNo)
                .HasMaxLength(35)
                .HasColumnName("External Document No_");
            entity.Property(e => e.GenBusPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Gen_ Bus_ Posting Group");
            entity.Property(e => e.InvoiceDiscCode)
                .HasMaxLength(20)
                .HasColumnName("Invoice Disc_ Code");
            entity.Property(e => e.LanguageCode)
                .HasMaxLength(10)
                .HasColumnName("Language Code");
            entity.Property(e => e.LocationCode)
                .HasMaxLength(10)
                .HasColumnName("Location Code");
            entity.Property(e => e.MemberCardNo)
                .HasMaxLength(100)
                .HasColumnName("Member Card No_");
            entity.Property(e => e.MobilePhoneNo)
                .HasMaxLength(30)
                .HasColumnName("Mobile Phone No_");
            entity.Property(e => e.NoPrinted).HasColumnName("No_ Printed");
            entity.Property(e => e.NoSeries)
                .HasMaxLength(10)
                .HasColumnName("No_ Series");
            entity.Property(e => e.OnHold)
                .HasMaxLength(3)
                .HasColumnName("On Hold");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("Order Date");
            entity.Property(e => e.OrderNo)
                .HasMaxLength(20)
                .HasColumnName("Order No_");
            entity.Property(e => e.OrderNoSeries)
                .HasMaxLength(10)
                .HasColumnName("Order No_ Series");
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
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(30)
                .HasColumnName("Phone No_");
            entity.Property(e => e.PmtDiscountDate)
                .HasColumnType("datetime")
                .HasColumnName("Pmt_ Discount Date");
            entity.Property(e => e.PostingDate)
                .HasColumnType("datetime")
                .HasColumnName("Posting Date");
            entity.Property(e => e.PostingDescription)
                .HasMaxLength(50)
                .HasColumnName("Posting Description");
            entity.Property(e => e.PricesIncludingVat).HasColumnName("Prices Including VAT");
            entity.Property(e => e.PromisedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Promised Delivery Date");
            entity.Property(e => e.QuoteNo)
                .HasMaxLength(20)
                .HasColumnName("Quote No_");
            entity.Property(e => e.ReasonCode)
                .HasMaxLength(10)
                .HasColumnName("Reason Code");
            entity.Property(e => e.RecivingPickingNo)
                .HasMaxLength(20)
                .HasColumnName("Reciving_Picking No_");
            entity.Property(e => e.RequestedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Requested Delivery Date");
            entity.Property(e => e.ResponsibilityCenter)
                .HasMaxLength(10)
                .HasColumnName("Responsibility Center");
            entity.Property(e => e.RetailSpecialOrder).HasColumnName("Retail Special Order");
            entity.Property(e => e.RetailStatus).HasColumnName("Retail Status");
            entity.Property(e => e.RetailZonesCode)
                .HasMaxLength(10)
                .HasColumnName("Retail Zones Code");
            entity.Property(e => e.RetailZonesDescription)
                .HasMaxLength(30)
                .HasColumnName("Retail Zones Description");
            entity.Property(e => e.RingBackDate)
                .HasColumnType("datetime")
                .HasColumnName("Ring Back Date");
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
            entity.Property(e => e.SellToPostCode)
                .HasMaxLength(20)
                .HasColumnName("Sell-to Post Code");
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
            entity.Property(e => e.ShipToHouseNo)
                .HasMaxLength(30)
                .HasColumnName("Ship-to House No_");
            entity.Property(e => e.ShipToName)
                .HasMaxLength(50)
                .HasColumnName("Ship-to Name");
            entity.Property(e => e.ShipToName2)
                .HasMaxLength(50)
                .HasColumnName("Ship-to Name 2");
            entity.Property(e => e.ShipToPostCode)
                .HasMaxLength(20)
                .HasColumnName("Ship-to Post Code");
            entity.Property(e => e.ShipToTelephone)
                .HasMaxLength(30)
                .HasColumnName("Ship-To Telephone");
            entity.Property(e => e.ShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Shipment Date");
            entity.Property(e => e.ShipmentMethodCode)
                .HasMaxLength(10)
                .HasColumnName("Shipment Method Code");
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
            entity.Property(e => e.SourceCode)
                .HasMaxLength(10)
                .HasColumnName("Source Code");
            entity.Property(e => e.SpecialOrderOrigin).HasColumnName("Special Order Origin");
            entity.Property(e => e.SpecialVatRateEnabled).HasColumnName("Special VAT Rate Enabled");
            entity.Property(e => e.SpoCreatedEntry).HasColumnName("SPO-Created Entry");
            entity.Property(e => e.StatementNo)
                .HasMaxLength(20)
                .HasColumnName("Statement No_");
            entity.Property(e => e.StoreNo)
                .HasMaxLength(10)
                .HasColumnName("Store No_");
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
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("User ID");
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

        modelBuilder.Entity<MikesportCoSALSalesShipmentLine>(entity =>
        {
            entity.HasKey(e => new { e.DocumentNo, e.LineNo }).HasName("Mikesport & Co_ S_A_L_$Sales Shipment Line$0");

            entity.ToTable("Mikesport & Co_ S_A_L_$Sales Shipment Line");

            entity.HasIndex(e => new { e.OrderNo, e.OrderLineNo, e.DocumentNo, e.LineNo }, "$1").IsUnique();

            entity.HasIndex(e => new { e.BlanketOrderNo, e.BlanketOrderLineNo, e.DocumentNo, e.LineNo }, "$2").IsUnique();

            entity.HasIndex(e => new { e.ItemShptEntryNo, e.DocumentNo, e.LineNo }, "$3").IsUnique();

            entity.HasIndex(e => new { e.SellToCustomerNo, e.DocumentNo, e.LineNo }, "$4").IsUnique();

            entity.HasIndex(e => new { e.BillToCustomerNo, e.DocumentNo, e.LineNo }, "$5").IsUnique();

            entity.HasIndex(e => new { e.DivisionCode, e.ItemCategoryCode, e.ProductGroupCode, e.ItemFamilyCode, e.ItemBrandCode, e.PriceRangeCode, e.ItemGenderLevel1Code, e.ItemGenderLevel2Code, e.SeasonCode, e.ItemLineCode, e.IsConsigned, e.DocumentNo, e.LineNo }, "$6").IsUnique();

            entity.Property(e => e.DocumentNo)
                .HasMaxLength(20)
                .HasColumnName("Document No_");
            entity.Property(e => e.LineNo).HasColumnName("Line No_");
            entity.Property(e => e.AllowInvoiceDisc).HasColumnName("Allow Invoice Disc_");
            entity.Property(e => e.AllowLineDisc).HasColumnName("Allow Line Disc_");
            entity.Property(e => e.AmountIncludingVatLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Amount Including VAT (LCY)");
            entity.Property(e => e.ApplFromItemEntry).HasColumnName("Appl_-from Item Entry");
            entity.Property(e => e.ApplToItemEntry).HasColumnName("Appl_-to Item Entry");
            entity.Property(e => e.Area).HasMaxLength(10);
            entity.Property(e => e.AttachedToLineNo).HasColumnName("Attached to Line No_");
            entity.Property(e => e.AuthorizedForCreditCard).HasColumnName("Authorized for Credit Card");
            entity.Property(e => e.AutoAccGroup)
                .HasMaxLength(10)
                .HasColumnName("Auto_ Acc_ Group");
            entity.Property(e => e.BarcodeNo)
                .HasMaxLength(20)
                .HasColumnName("Barcode No_");
            entity.Property(e => e.BillToCustomerNo)
                .HasMaxLength(20)
                .HasColumnName("Bill-to Customer No_");
            entity.Property(e => e.BinCode)
                .HasMaxLength(20)
                .HasColumnName("Bin Code");
            entity.Property(e => e.BlanketOrderLineNo).HasColumnName("Blanket Order Line No_");
            entity.Property(e => e.BlanketOrderNo)
                .HasMaxLength(20)
                .HasColumnName("Blanket Order No_");
            entity.Property(e => e.ConfigurationId)
                .HasMaxLength(30)
                .HasColumnName("Configuration ID");
            entity.Property(e => e.CrossReferenceNo)
                .HasMaxLength(20)
                .HasColumnName("Cross-Reference No_");
            entity.Property(e => e.CrossReferenceType).HasColumnName("Cross-Reference Type");
            entity.Property(e => e.CrossReferenceTypeNo)
                .HasMaxLength(30)
                .HasColumnName("Cross-Reference Type No_");
            entity.Property(e => e.CustomerDiscGroup)
                .HasMaxLength(20)
                .HasColumnName("Customer Disc_ Group");
            entity.Property(e => e.CustomerPriceGroup)
                .HasMaxLength(10)
                .HasColumnName("Customer Price Group");
            entity.Property(e => e.DeliverFrom).HasColumnName("Deliver from");
            entity.Property(e => e.DeliveringMethod).HasColumnName("Delivering Method");
            entity.Property(e => e.DeliveryDateTime)
                .HasColumnType("datetime")
                .HasColumnName("Delivery Date Time");
            entity.Property(e => e.DeliveryLocationCode)
                .HasMaxLength(10)
                .HasColumnName("Delivery Location Code");
            entity.Property(e => e.DeliveryReferenceNo)
                .HasMaxLength(30)
                .HasColumnName("Delivery Reference No");
            entity.Property(e => e.DeliveryUserId)
                .HasMaxLength(50)
                .HasColumnName("Delivery User ID");
            entity.Property(e => e.DeprUntilFaPostingDate).HasColumnName("Depr_ until FA Posting Date");
            entity.Property(e => e.DepreciationBookCode)
                .HasMaxLength(10)
                .HasColumnName("Depreciation Book Code");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Description2)
                .HasMaxLength(50)
                .HasColumnName("Description 2");
            entity.Property(e => e.DimensionSetId).HasColumnName("Dimension Set ID");
            entity.Property(e => e.DivisionCode)
                .HasMaxLength(10)
                .HasColumnName("Division Code");
            entity.Property(e => e.DropShipment).HasColumnName("Drop Shipment");
            entity.Property(e => e.DuplicateInDepreciationBook)
                .HasMaxLength(10)
                .HasColumnName("Duplicate in Depreciation Book");
            entity.Property(e => e.EstimatedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Estimated Delivery Date");
            entity.Property(e => e.ExcludeZeroPricePrinting).HasColumnName("Exclude Zero Price Printing");
            entity.Property(e => e.ExitPoint)
                .HasMaxLength(10)
                .HasColumnName("Exit Point");
            entity.Property(e => e.FaPostingDate)
                .HasColumnType("datetime")
                .HasColumnName("FA Posting Date");
            entity.Property(e => e.GenBusPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Gen_ Bus_ Posting Group");
            entity.Property(e => e.GenProdPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Gen_ Prod_ Posting Group");
            entity.Property(e => e.GrossWeight)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Gross Weight");
            entity.Property(e => e.IsConsigned).HasColumnName("Is Consigned");
            entity.Property(e => e.ItemBrandCode)
                .HasMaxLength(20)
                .HasColumnName("Item Brand Code");
            entity.Property(e => e.ItemCategoryCode)
                .HasMaxLength(10)
                .HasColumnName("Item Category Code");
            entity.Property(e => e.ItemChargeBaseAmount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Item Charge Base Amount");
            entity.Property(e => e.ItemFamilyCode)
                .HasMaxLength(10)
                .HasColumnName("Item Family Code");
            entity.Property(e => e.ItemGenderLevel1Code)
                .HasMaxLength(20)
                .HasColumnName("Item Gender Level 1 Code");
            entity.Property(e => e.ItemGenderLevel2Code)
                .HasMaxLength(20)
                .HasColumnName("Item Gender Level 2 Code");
            entity.Property(e => e.ItemLineCode)
                .HasMaxLength(20)
                .HasColumnName("Item Line Code");
            entity.Property(e => e.ItemShptEntryNo).HasColumnName("Item Shpt_ Entry No_");
            entity.Property(e => e.ItemTrackingNo)
                .HasMaxLength(20)
                .HasColumnName("Item Tracking No_");
            entity.Property(e => e.JobContractEntryNo).HasColumnName("Job Contract Entry No_");
            entity.Property(e => e.JobNo)
                .HasMaxLength(20)
                .HasColumnName("Job No_");
            entity.Property(e => e.JobTaskNo)
                .HasMaxLength(20)
                .HasColumnName("Job Task No_");
            entity.Property(e => e.LineDiscount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Line Discount _");
            entity.Property(e => e.LocationCode)
                .HasMaxLength(10)
                .HasColumnName("Location Code");
            entity.Property(e => e.NetWeight)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Net Weight");
            entity.Property(e => e.No)
                .HasMaxLength(20)
                .HasColumnName("No_");
            entity.Property(e => e.NoLaterThanDate)
                .HasColumnType("datetime")
                .HasColumnName("No later than Date");
            entity.Property(e => e.OptionValueText)
                .HasMaxLength(100)
                .HasColumnName("Option Value Text");
            entity.Property(e => e.OrderLineNo).HasColumnName("Order Line No_");
            entity.Property(e => e.OrderNo)
                .HasMaxLength(20)
                .HasColumnName("Order No_");
            entity.Property(e => e.OutboundWhseHandlingTime)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Outbound Whse_ Handling Time");
            entity.Property(e => e.PlannedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Planned Delivery Date");
            entity.Property(e => e.PlannedShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Planned Shipment Date");
            entity.Property(e => e.PostingDate)
                .HasColumnType("datetime")
                .HasColumnName("Posting Date");
            entity.Property(e => e.PostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Posting Group");
            entity.Property(e => e.PriceRangeCode)
                .HasMaxLength(20)
                .HasColumnName("Price Range Code");
            entity.Property(e => e.ProductGroupCode)
                .HasMaxLength(10)
                .HasColumnName("Product Group Code");
            entity.Property(e => e.PromisedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Promised Delivery Date");
            entity.Property(e => e.PurchOrderLineNo).HasColumnName("Purch_ Order Line No_");
            entity.Property(e => e.PurchaseOrderNo)
                .HasMaxLength(20)
                .HasColumnName("Purchase Order No_");
            entity.Property(e => e.PurchasingCode)
                .HasMaxLength(10)
                .HasColumnName("Purchasing Code");
            entity.Property(e => e.QtyInvoicedBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Invoiced (Base)");
            entity.Property(e => e.QtyPerUnitOfMeasure)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ per Unit of Measure");
            entity.Property(e => e.QtyShippedNotInvoiced)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Shipped Not Invoiced");
            entity.Property(e => e.Quantity).HasColumnType("decimal(38, 20)");
            entity.Property(e => e.QuantityBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Quantity (Base)");
            entity.Property(e => e.QuantityInvoiced)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Quantity Invoiced");
            entity.Property(e => e.RequestedDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Requested Delivery Date");
            entity.Property(e => e.ResponsibilityCenter)
                .HasMaxLength(10)
                .HasColumnName("Responsibility Center");
            entity.Property(e => e.RetailSpecialOrder).HasColumnName("Retail Special Order");
            entity.Property(e => e.ReturnPolicy).HasColumnName("Return Policy");
            entity.Property(e => e.ReturnReasonCode)
                .HasMaxLength(10)
                .HasColumnName("Return Reason Code");
            entity.Property(e => e.SeasonCode)
                .HasMaxLength(10)
                .HasColumnName("Season Code");
            entity.Property(e => e.SellToCustomerNo)
                .HasMaxLength(20)
                .HasColumnName("Sell-to Customer No_");
            entity.Property(e => e.ShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Shipment Date");
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
            entity.Property(e => e.SpoDocumentMethod).HasColumnName("SPO Document Method");
            entity.Property(e => e.SpoWhseLocation)
                .HasMaxLength(10)
                .HasColumnName("SPO Whse Location");
            entity.Property(e => e.StoreSalesLocation)
                .HasMaxLength(10)
                .HasColumnName("Store Sales Location");
            entity.Property(e => e.TaxAreaCode)
                .HasMaxLength(20)
                .HasColumnName("Tax Area Code");
            entity.Property(e => e.TaxGroupCode)
                .HasMaxLength(10)
                .HasColumnName("Tax Group Code");
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
            entity.Property(e => e.UnitCost)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Cost");
            entity.Property(e => e.UnitCostLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Cost (LCY)");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure");
            entity.Property(e => e.UnitOfMeasureCode)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure Code");
            entity.Property(e => e.UnitOfMeasureCrossRef)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure (Cross Ref_)");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Price");
            entity.Property(e => e.UnitVolume)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Volume");
            entity.Property(e => e.UnitsPerParcel)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Units per Parcel");
            entity.Property(e => e.UseDuplicationList).HasColumnName("Use Duplication List");
            entity.Property(e => e.VariantCode)
                .HasMaxLength(10)
                .HasColumnName("Variant Code");
            entity.Property(e => e.Vat)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("VAT _");
            entity.Property(e => e.VatAmntInOriginalCurrency)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("VAT Amnt In Original Currency");
            entity.Property(e => e.VatAmountInLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("VAT Amount In LCY");
            entity.Property(e => e.VatBaseAmount)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("VAT Base Amount");
            entity.Property(e => e.VatBusPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("VAT Bus_ Posting Group");
            entity.Property(e => e.VatCalculationType).HasColumnName("VAT Calculation Type");
            entity.Property(e => e.VatProdPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("VAT Prod_ Posting Group");
            entity.Property(e => e.VendorDeliversTo).HasColumnName("Vendor Delivers to");
            entity.Property(e => e.VendorNo)
                .HasMaxLength(20)
                .HasColumnName("Vendor No_");
            entity.Property(e => e.WarrantyPeriod)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Warranty Period");
            entity.Property(e => e.WorkTypeCode)
                .HasMaxLength(10)
                .HasColumnName("Work Type Code");
        });

        modelBuilder.Entity<MikesportCoSALTransferHeader>(entity =>
        {
            entity.HasKey(e => e.No).HasName("Mikesport & Co_ S_A_L_$Transfer Header$0");

            entity.ToTable("Mikesport & Co_ S_A_L_$Transfer Header");

            entity.HasIndex(e => new { e.RetailStatus, e.TransferFromCode, e.No }, "$1").IsUnique();

            entity.HasIndex(e => new { e.RetailStatus, e.TransferToCode, e.No }, "$2").IsUnique();

            entity.HasIndex(e => new { e.TransferType, e.ExternalDocumentNo, e.TransferToCode, e.No }, "$3").IsUnique();

            entity.HasIndex(e => new { e.AllocationStatus, e.No }, "$4").IsUnique();

            entity.HasIndex(e => new { e.BuyerId, e.BuyerGroupCode, e.CreatedBySourceCode, e.No }, "$5").IsUnique();

            entity.HasIndex(e => new { e.RetailSpecialOrder, e.SpoTransferStatus, e.No }, "$6").IsUnique();

            entity.HasIndex(e => new { e.ReplicationCounter, e.No }, "$7").IsUnique();

            entity.Property(e => e.No)
                .HasMaxLength(20)
                .HasColumnName("No_");
            entity.Property(e => e.AllocationStatus).HasColumnName("Allocation Status");
            entity.Property(e => e.Area).HasMaxLength(10);
            entity.Property(e => e.AssignedUserId)
                .HasMaxLength(50)
                .HasColumnName("Assigned User ID");
            entity.Property(e => e.BuyerGroupCode)
                .HasMaxLength(10)
                .HasColumnName("Buyer Group Code");
            entity.Property(e => e.BuyerId)
                .HasMaxLength(50)
                .HasColumnName("Buyer ID");
            entity.Property(e => e.CreatedBySourceCode)
                .HasMaxLength(10)
                .HasColumnName("Created By Source Code");
            entity.Property(e => e.CreatedByUser)
                .HasMaxLength(50)
                .HasColumnName("Created By User");
            entity.Property(e => e.DateTimeCreated)
                .HasColumnType("datetime")
                .HasColumnName("Date _ Time Created");
            entity.Property(e => e.DimensionSetId).HasColumnName("Dimension Set ID");
            entity.Property(e => e.EntryExitPoint)
                .HasMaxLength(10)
                .HasColumnName("Entry_Exit Point");
            entity.Property(e => e.ExternalDocumentNo)
                .HasMaxLength(35)
                .HasColumnName("External Document No_");
            entity.Property(e => e.InStoreCreatedEntry).HasColumnName("InStore-Created Entry");
            entity.Property(e => e.InTransitCode)
                .HasMaxLength(10)
                .HasColumnName("In-Transit Code");
            entity.Property(e => e.InboundWhseHandlingTime)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Inbound Whse_ Handling Time");
            entity.Property(e => e.IntegrationStatus).HasColumnName("Integration Status");
            entity.Property(e => e.LastDateTimeModified)
                .HasColumnType("datetime")
                .HasColumnName("Last Date _ Time Modified");
            entity.Property(e => e.LastModifiedByUser)
                .HasMaxLength(50)
                .HasColumnName("Last Modified by User");
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
            entity.Property(e => e.PreReceiveReferenceNo)
                .HasMaxLength(20)
                .HasColumnName("Pre Receive Reference No_");
            entity.Property(e => e.PurchaseOrderNo)
                .HasMaxLength(20)
                .HasColumnName("Purchase Order No_");
            entity.Property(e => e.ReceiptDate)
                .HasColumnType("datetime")
                .HasColumnName("Receipt Date");
            entity.Property(e => e.ReceivingTransFromCode)
                .HasMaxLength(10)
                .HasColumnName("Receiving Trans-From Code");
            entity.Property(e => e.ReceivingTransToCode)
                .HasMaxLength(10)
                .HasColumnName("Receiving Trans-To Code");
            entity.Property(e => e.RecivingPickingNo)
                .HasMaxLength(20)
                .HasColumnName("Reciving_Picking No_");
            entity.Property(e => e.ReplicationCounter).HasColumnName("Replication Counter");
            entity.Property(e => e.RetailSpecialOrder).HasColumnName("Retail Special Order");
            entity.Property(e => e.RetailStatus).HasColumnName("Retail Status");
            entity.Property(e => e.RtcFilteringType).HasColumnName("RTC Filtering Type");
            entity.Property(e => e.SendToWms).HasColumnName("Send To WMS");
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
            entity.Property(e => e.SpoCreatedEntry).HasColumnName("SPO-Created Entry");
            entity.Property(e => e.SpoTransferStatus).HasColumnName("SPO Transfer Status");
            entity.Property(e => e.StoreFrom)
                .HasMaxLength(10)
                .HasColumnName("Store-from");
            entity.Property(e => e.StoreTo)
                .HasMaxLength(10)
                .HasColumnName("Store-to");
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
            entity.Property(e => e.TransferFromDepartCode)
                .HasMaxLength(20)
                .HasColumnName("Transfer-from Depart_ Code");
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
            entity.Property(e => e.TransferToDepartCode)
                .HasMaxLength(20)
                .HasColumnName("Transfer-to Depart_ Code");
            entity.Property(e => e.TransferToName)
                .HasMaxLength(50)
                .HasColumnName("Transfer-to Name");
            entity.Property(e => e.TransferToName2)
                .HasMaxLength(50)
                .HasColumnName("Transfer-to Name 2");
            entity.Property(e => e.TransferToPostCode)
                .HasMaxLength(20)
                .HasColumnName("Transfer-to Post Code");
            entity.Property(e => e.TransferType).HasColumnName("Transfer Type");
            entity.Property(e => e.TransportMethod)
                .HasMaxLength(10)
                .HasColumnName("Transport Method");
            entity.Property(e => e.TrsfFromCountryRegionCode)
                .HasMaxLength(10)
                .HasColumnName("Trsf_-from Country_Region Code");
            entity.Property(e => e.TrsfToCountryRegionCode)
                .HasMaxLength(10)
                .HasColumnName("Trsf_-to Country_Region Code");
            entity.Property(e => e.VendorNo)
                .HasMaxLength(20)
                .HasColumnName("Vendor No_");
        });

        modelBuilder.Entity<MikesportCoSALTransferLine>(entity =>
        {
            entity.HasKey(e => new { e.DocumentNo, e.LineNo }).HasName("Mikesport & Co_ S_A_L_$Transfer Line$0");

            entity.ToTable("Mikesport & Co_ S_A_L_$Transfer Line");

            entity.HasIndex(e => new { e.TransferToCode, e.Status, e.DerivedFromLineNo, e.ItemNo, e.VariantCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.ReceiptDate, e.InTransitCode, e.DocumentNo, e.LineNo }, "$1").IsUnique();

            entity.HasIndex(e => new { e.TransferFromCode, e.Status, e.DerivedFromLineNo, e.ItemNo, e.VariantCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.ShipmentDate, e.InTransitCode, e.DocumentNo, e.LineNo }, "$2").IsUnique();

            entity.HasIndex(e => new { e.ItemNo, e.DocumentNo, e.LineNo }, "$3").IsUnique();

            entity.HasIndex(e => new { e.TransferType, e.PurchaseOrderNo, e.TransferToCode, e.ItemNo, e.VariantCode, e.DocumentNo, e.LineNo }, "$4").IsUnique();

            entity.HasIndex(e => new { e.Division, e.ItemCategoryCode, e.ProductGroupCode, e.ItemFamilyCode, e.ItemBrandCode, e.PriceRangeCode, e.ItemGenderLevel1Code, e.ItemGenderLevel2Code, e.SeasonCode, e.ItemLineCode, e.IsConsigned, e.DocumentNo, e.LineNo }, "$5").IsUnique();

            entity.HasIndex(e => new { e.ReplicationCounter, e.DocumentNo, e.LineNo }, "$6").IsUnique();

            entity.Property(e => e.DocumentNo)
                .HasMaxLength(20)
                .HasColumnName("Document No_");
            entity.Property(e => e.LineNo).HasColumnName("Line No_");
            entity.Property(e => e.ActualQtyToReceive)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Actual Qty_ to Receive");
            entity.Property(e => e.ActualQtyToReceiveBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Actual Qty_ to Receive (Base)");
            entity.Property(e => e.BarcodeNo)
                .HasMaxLength(20)
                .HasColumnName("Barcode No_");
            entity.Property(e => e.CompletelyReceived).HasColumnName("Completely Received");
            entity.Property(e => e.CompletelyShipped).HasColumnName("Completely Shipped");
            entity.Property(e => e.ConfigurationId)
                .HasMaxLength(30)
                .HasColumnName("Configuration ID");
            entity.Property(e => e.DerivedFromLineNo).HasColumnName("Derived From Line No_");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Description2)
                .HasMaxLength(100)
                .HasColumnName("Description 2");
            entity.Property(e => e.DimensionSetId).HasColumnName("Dimension Set ID");
            entity.Property(e => e.Division).HasMaxLength(10);
            entity.Property(e => e.ExcludeZeroPricePrinting).HasColumnName("Exclude Zero Price Printing");
            entity.Property(e => e.GenProdPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Gen_ Prod_ Posting Group");
            entity.Property(e => e.GrossWeight)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Gross Weight");
            entity.Property(e => e.InStoreDocumentStatus).HasColumnName("InStore Document Status");
            entity.Property(e => e.InTransitCode)
                .HasMaxLength(10)
                .HasColumnName("In-Transit Code");
            entity.Property(e => e.InboundWhseHandlingTime)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Inbound Whse_ Handling Time");
            entity.Property(e => e.InventoryPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Inventory Posting Group");
            entity.Property(e => e.IsConsigned).HasColumnName("Is Consigned");
            entity.Property(e => e.ItemBrandCode)
                .HasMaxLength(20)
                .HasColumnName("Item Brand Code");
            entity.Property(e => e.ItemCategoryCode)
                .HasMaxLength(10)
                .HasColumnName("Item Category Code");
            entity.Property(e => e.ItemFamilyCode)
                .HasMaxLength(10)
                .HasColumnName("Item Family Code");
            entity.Property(e => e.ItemGenderLevel1Code)
                .HasMaxLength(20)
                .HasColumnName("Item Gender Level 1 Code");
            entity.Property(e => e.ItemGenderLevel2Code)
                .HasMaxLength(20)
                .HasColumnName("Item Gender Level 2 Code");
            entity.Property(e => e.ItemLineCode)
                .HasMaxLength(20)
                .HasColumnName("Item Line Code");
            entity.Property(e => e.ItemNo)
                .HasMaxLength(20)
                .HasColumnName("Item No_");
            entity.Property(e => e.NetWeight)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Net Weight");
            entity.Property(e => e.OutboundWhseHandlingTime)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Outbound Whse_ Handling Time");
            entity.Property(e => e.OutstandingQtyBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Outstanding Qty_ (Base)");
            entity.Property(e => e.OutstandingQuantity)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Outstanding Quantity");
            entity.Property(e => e.PlanningFlexibility).HasColumnName("Planning Flexibility");
            entity.Property(e => e.PriceRangeCode)
                .HasMaxLength(20)
                .HasColumnName("Price Range Code");
            entity.Property(e => e.ProductGroupCode)
                .HasMaxLength(10)
                .HasColumnName("Product Group Code");
            entity.Property(e => e.PurchaseOrderNo)
                .HasMaxLength(20)
                .HasColumnName("Purchase Order No_");
            entity.Property(e => e.QtyDifference)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Difference");
            entity.Property(e => e.QtyDifferenceBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Difference (Base)");
            entity.Property(e => e.QtyInTransit)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ in Transit");
            entity.Property(e => e.QtyInTransitBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ in Transit (Base)");
            entity.Property(e => e.QtyPerUnitOfMeasure)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ per Unit of Measure");
            entity.Property(e => e.QtyReceivedBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Received (Base)");
            entity.Property(e => e.QtyShippedBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ Shipped (Base)");
            entity.Property(e => e.QtyToReceive)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Receive");
            entity.Property(e => e.QtyToReceiveBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Receive (Base)");
            entity.Property(e => e.QtyToShip)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Ship");
            entity.Property(e => e.QtyToShipBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ to Ship (Base)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(38, 20)");
            entity.Property(e => e.QuantityBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Quantity (Base)");
            entity.Property(e => e.QuantityReceived)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Quantity Received");
            entity.Property(e => e.QuantityShipped)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Quantity Shipped");
            entity.Property(e => e.ReasonCode)
                .HasMaxLength(10)
                .HasColumnName("Reason Code");
            entity.Property(e => e.ReceiptDate)
                .HasColumnType("datetime")
                .HasColumnName("Receipt Date");
            entity.Property(e => e.ReceivingTransFromCode)
                .HasMaxLength(10)
                .HasColumnName("Receiving Trans-From Code");
            entity.Property(e => e.ReceivingTransToCode)
                .HasMaxLength(10)
                .HasColumnName("Receiving Trans-To Code");
            entity.Property(e => e.ReplicationCounter).HasColumnName("Replication Counter");
            entity.Property(e => e.SeasonCode)
                .HasMaxLength(10)
                .HasColumnName("Season Code");
            entity.Property(e => e.SendToWms).HasColumnName("Send To WMS");
            entity.Property(e => e.ShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Shipment Date");
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
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TransferFromBinCode)
                .HasMaxLength(20)
                .HasColumnName("Transfer-from Bin Code");
            entity.Property(e => e.TransferFromCode)
                .HasMaxLength(10)
                .HasColumnName("Transfer-from Code");
            entity.Property(e => e.TransferToBinCode)
                .HasMaxLength(20)
                .HasColumnName("Transfer-To Bin Code");
            entity.Property(e => e.TransferToCode)
                .HasMaxLength(10)
                .HasColumnName("Transfer-to Code");
            entity.Property(e => e.TransferType).HasColumnName("Transfer Type");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure");
            entity.Property(e => e.UnitOfMeasureCode)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure Code");
            entity.Property(e => e.UnitVolume)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Volume");
            entity.Property(e => e.UnitsPerParcel)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Units per Parcel");
            entity.Property(e => e.VariantCode)
                .HasMaxLength(10)
                .HasColumnName("Variant Code");
            entity.Property(e => e.WarrantyPeriod)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Warranty Period");
        });

        modelBuilder.Entity<MikesportCoSALTransferShipmentHeader>(entity =>
        {
            entity.HasMany(header => header.Lines)
                .WithOne(line => line.Header)
                .HasForeignKey(line => line.DocumentNo)
                .HasPrincipalKey(header => header.No);
            
            entity.HasKey(e => e.No).HasName("Mikesport & Co_ S_A_L_$Transfer Shipment Header$0");

            entity.ToTable("Mikesport & Co_ S_A_L_$Transfer Shipment Header");

            entity.HasIndex(e => new { e.RecivingPickingNo, e.No }, "$1").IsUnique();

            entity.HasIndex(e => new { e.TransferFromCode, e.No }, "$2").IsUnique();

            entity.HasIndex(e => new { e.TransferOrderNo, e.No }, "$3").IsUnique();

            entity.Property(e => e.No)
                .HasMaxLength(20)
                .HasColumnName("No_");
            entity.Property(e => e.Area).HasMaxLength(10);
            entity.Property(e => e.CreatedByUser)
                .HasMaxLength(50)
                .HasColumnName("Created By User");
            entity.Property(e => e.DateTimeCreated)
                .HasColumnType("datetime")
                .HasColumnName("Date _ Time Created");
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
            entity.Property(e => e.LastDateTimeModified)
                .HasColumnType("datetime")
                .HasColumnName("Last Date _ Time Modified");
            entity.Property(e => e.LastModifiedByUser)
                .HasMaxLength(50)
                .HasColumnName("Last Modified by User");
            entity.Property(e => e.NoSeries)
                .HasMaxLength(10)
                .HasColumnName("No_ Series");
            entity.Property(e => e.PostingDate)
                .HasColumnType("datetime")
                .HasColumnName("Posting Date");
            entity.Property(e => e.PurchaseOrderNo)
                .HasMaxLength(20)
                .HasColumnName("Purchase Order No_");
            entity.Property(e => e.ReceiptDate)
                .HasColumnType("datetime")
                .HasColumnName("Receipt Date");
            entity.Property(e => e.RecivingPickingNo)
                .HasMaxLength(20)
                .HasColumnName("Reciving_Picking No_");
            entity.Property(e => e.RetailStatus).HasColumnName("Retail Status");
            entity.Property(e => e.ShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Shipment Date");
            entity.Property(e => e.ShipmentMethodCode)
                .HasMaxLength(10)
                .HasColumnName("Shipment Method Code");
            entity.Property(e => e.ShippingAgentCode)
                .HasMaxLength(10)
                .HasColumnName("Shipping Agent Code");
            entity.Property(e => e.ShippingAgentServiceCode)
                .HasMaxLength(10)
                .HasColumnName("Shipping Agent Service Code");
            entity.Property(e => e.ShortcutDimension1Code)
                .HasMaxLength(20)
                .HasColumnName("Shortcut Dimension 1 Code");
            entity.Property(e => e.ShortcutDimension2Code)
                .HasMaxLength(20)
                .HasColumnName("Shortcut Dimension 2 Code");
            entity.Property(e => e.StoreFrom)
                .HasMaxLength(10)
                .HasColumnName("Store-from");
            entity.Property(e => e.StoreTo)
                .HasMaxLength(10)
                .HasColumnName("Store-to");
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
            entity.Property(e => e.TransferOrderDate)
                .HasColumnType("datetime")
                .HasColumnName("Transfer Order Date");
            entity.Property(e => e.TransferOrderNo)
                .HasMaxLength(20)
                .HasColumnName("Transfer Order No_");
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

        modelBuilder.Entity<MikesportCoSALTransferShipmentLine>(entity =>
        {
            entity.HasKey(e => new { e.DocumentNo, e.LineNo }).HasName("Mikesport & Co_ S_A_L_$Transfer Shipment Line$0");

            entity.ToTable("Mikesport & Co_ S_A_L_$Transfer Shipment Line");

            entity.HasIndex(e => new { e.TransferOrderNo, e.ItemNo, e.ShipmentDate, e.DocumentNo, e.LineNo }, "$1").IsUnique();

            entity.HasIndex(e => new { e.Division, e.ItemCategoryCode, e.ProductGroupCode, e.ItemFamilyCode, e.ItemBrandCode, e.PriceRangeCode, e.ItemGenderLevel1Code, e.ItemGenderLevel2Code, e.SeasonCode, e.ItemLineCode, e.IsConsigned, e.DocumentNo, e.LineNo }, "$2").IsUnique();

            entity.Property(e => e.DocumentNo)
                .HasMaxLength(20)
                .HasColumnName("Document No_");
            entity.Property(e => e.LineNo).HasColumnName("Line No_");
            entity.Property(e => e.BarcodeNo)
                .HasMaxLength(20)
                .HasColumnName("Barcode No_");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Description2)
                .HasMaxLength(100)
                .HasColumnName("Description 2");
            entity.Property(e => e.DimensionSetId).HasColumnName("Dimension Set ID");
            entity.Property(e => e.Division).HasMaxLength(10);
            entity.Property(e => e.ExcludeZeroPricePrinting).HasColumnName("Exclude Zero Price Printing");
            entity.Property(e => e.GenProdPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Gen_ Prod_ Posting Group");
            entity.Property(e => e.GrossWeight)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Gross Weight");
            entity.Property(e => e.InTransitCode)
                .HasMaxLength(10)
                .HasColumnName("In-Transit Code");
            entity.Property(e => e.InventoryPostingGroup)
                .HasMaxLength(10)
                .HasColumnName("Inventory Posting Group");
            entity.Property(e => e.IsConsigned).HasColumnName("Is Consigned");
            entity.Property(e => e.ItemBrandCode)
                .HasMaxLength(20)
                .HasColumnName("Item Brand Code");
            entity.Property(e => e.ItemCategoryCode)
                .HasMaxLength(10)
                .HasColumnName("Item Category Code");
            entity.Property(e => e.ItemFamilyCode)
                .HasMaxLength(10)
                .HasColumnName("Item Family Code");
            entity.Property(e => e.ItemGenderLevel1Code)
                .HasMaxLength(20)
                .HasColumnName("Item Gender Level 1 Code");
            entity.Property(e => e.ItemGenderLevel2Code)
                .HasMaxLength(20)
                .HasColumnName("Item Gender Level 2 Code");
            entity.Property(e => e.ItemLineCode)
                .HasMaxLength(20)
                .HasColumnName("Item Line Code");
            entity.Property(e => e.ItemNo)
                .HasMaxLength(20)
                .HasColumnName("Item No_");
            entity.Property(e => e.ItemShptEntryNo).HasColumnName("Item Shpt_ Entry No_");
            entity.Property(e => e.NetWeight)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Net Weight");
            entity.Property(e => e.PriceRangeCode)
                .HasMaxLength(20)
                .HasColumnName("Price Range Code");
            entity.Property(e => e.ProductGroupCode)
                .HasMaxLength(10)
                .HasColumnName("Product Group Code");
            entity.Property(e => e.QtyPerUnitOfMeasure)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Qty_ per Unit of Measure");
            entity.Property(e => e.Quantity).HasColumnType("decimal(38, 20)");
            entity.Property(e => e.QuantityBase)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Quantity (Base)");
            entity.Property(e => e.ReasonCode)
                .HasMaxLength(10)
                .HasColumnName("Reason Code");
            entity.Property(e => e.SeasonCode)
                .HasMaxLength(10)
                .HasColumnName("Season Code");
            entity.Property(e => e.ShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("Shipment Date");
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
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TransferFromBinCode)
                .HasMaxLength(20)
                .HasColumnName("Transfer-from Bin Code");
            entity.Property(e => e.TransferFromCode)
                .HasMaxLength(10)
                .HasColumnName("Transfer-from Code");
            entity.Property(e => e.TransferOrderNo)
                .HasMaxLength(20)
                .HasColumnName("Transfer Order No_");
            entity.Property(e => e.TransferToCode)
                .HasMaxLength(10)
                .HasColumnName("Transfer-to Code");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure");
            entity.Property(e => e.UnitOfMeasureCode)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure Code");
            entity.Property(e => e.UnitVolume)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Volume");
            entity.Property(e => e.UnitsPerParcel)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Units per Parcel");
            entity.Property(e => e.VariantCode)
                .HasMaxLength(10)
                .HasColumnName("Variant Code");
            entity.Property(e => e.WarrantyPeriod)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Warranty Period");
        });
        
        modelBuilder.Entity<MikesportCoSALSalesPrice>(entity =>
        {
            entity.HasKey(e => new { e.ItemNo, e.SalesType, e.SalesCode, e.StartingDate, e.CurrencyCode, e.VariantCode, e.UnitOfMeasureCode, e.MinimumQuantity }).HasName("Mikesport & Co_ S_A_L_$Sales Price$0");

            entity.ToTable("Mikesport & Co_ S_A_L_$Sales Price");

            entity.HasIndex(e => new { e.SalesType, e.SalesCode, e.ItemNo, e.StartingDate, e.CurrencyCode, e.VariantCode, e.UnitOfMeasureCode, e.MinimumQuantity }, "$1").IsUnique();

            entity.Property(e => e.ItemNo)
                .HasMaxLength(20)
                .HasColumnName("Item No_");
            entity.Property(e => e.SalesType).HasColumnName("Sales Type");
            entity.Property(e => e.SalesCode)
                .HasMaxLength(20)
                .HasColumnName("Sales Code");
            entity.Property(e => e.StartingDate)
                .HasColumnType("datetime")
                .HasColumnName("Starting Date");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasColumnName("Currency Code");
            entity.Property(e => e.VariantCode)
                .HasMaxLength(10)
                .HasColumnName("Variant Code");
            entity.Property(e => e.UnitOfMeasureCode)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure Code");
            entity.Property(e => e.MinimumQuantity)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Minimum Quantity");
            entity.Property(e => e.AllowInvoiceDisc).HasColumnName("Allow Invoice Disc_");
            entity.Property(e => e.AllowLineDisc).HasColumnName("Allow Line Disc_");
            entity.Property(e => e.EndingDate)
                .HasColumnType("datetime")
                .HasColumnName("Ending Date");
            entity.Property(e => e.Markup)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Markup _");
            entity.Property(e => e.PriceIncludesVat).HasColumnName("Price Includes VAT");
            entity.Property(e => e.Profit)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Profit _");
            entity.Property(e => e.ProfitLcy)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Profit (LCY)");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Price");
            entity.Property(e => e.UnitPriceIncludingVat)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Price Including VAT");
            entity.Property(e => e.VatBusPostingGrPrice)
                .HasMaxLength(10)
                .HasColumnName("VAT Bus_ Posting Gr_ (Price)");
        });
        
        modelBuilder.Entity<MikesportCoSALDefaultSalesPrice>(entity =>
        {
            entity.HasKey(e => e.ItemNo).HasName("Mikesport & Co_ S_A_L_$Default Sales Price$0");

            entity.ToTable("Mikesport & Co_ S_A_L_$Default Sales Price");

            entity.Property(e => e.ItemNo)
                .HasMaxLength(20)
                .HasColumnName("Item No_");
            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.Description).HasMaxLength(60);
            entity.Property(e => e.DiscountPercentage)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Discount Percentage");
            entity.Property(e => e.DiscountedPrice)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Discounted Price");
            entity.Property(e => e.DiscountedPriceCurrency)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Discounted Price Currency");
            entity.Property(e => e.ReplicationCounter).HasColumnName("Replication Counter");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.UnitOfMeasureCode)
                .HasMaxLength(10)
                .HasColumnName("Unit of Measure Code");
            entity.Property(e => e.UnitPriceIncludingVat)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Price Including VAT");
            entity.Property(e => e.UnitPriceIncludingVatCurre)
                .HasColumnType("decimal(38, 20)")
                .HasColumnName("Unit Price Including VAT Curre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
}
