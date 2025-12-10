using System;
using System.Collections.Generic;

namespace MSWMS.Entities.External;

public partial class MikesportCoSALSalesShipmentHeader
{
    public byte[] Timestamp { get; set; } = null!;

    public string No { get; set; } = null!;

    public string SellToCustomerNo { get; set; } = null!;

    public string BillToCustomerNo { get; set; } = null!;

    public string BillToName { get; set; } = null!;

    public string BillToName2 { get; set; } = null!;

    public string BillToAddress { get; set; } = null!;

    public string BillToAddress2 { get; set; } = null!;

    public string BillToCity { get; set; } = null!;

    public string BillToContact { get; set; } = null!;

    public string YourReference { get; set; } = null!;

    public string ShipToCode { get; set; } = null!;

    public string ShipToName { get; set; } = null!;

    public string ShipToName2 { get; set; } = null!;

    public string ShipToAddress { get; set; } = null!;

    public string ShipToAddress2 { get; set; } = null!;

    public string ShipToCity { get; set; } = null!;

    public string ShipToContact { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public DateTime PostingDate { get; set; }

    public DateTime ShipmentDate { get; set; }

    public string PostingDescription { get; set; } = null!;

    public string PaymentTermsCode { get; set; } = null!;

    public DateTime DueDate { get; set; }

    public decimal PaymentDiscount { get; set; }

    public DateTime PmtDiscountDate { get; set; }

    public string ShipmentMethodCode { get; set; } = null!;

    public string LocationCode { get; set; } = null!;

    public string ShortcutDimension1Code { get; set; } = null!;

    public string ShortcutDimension2Code { get; set; } = null!;

    public string CustomerPostingGroup { get; set; } = null!;

    public string CurrencyCode { get; set; } = null!;

    public decimal CurrencyFactor { get; set; }

    public string CustomerPriceGroup { get; set; } = null!;

    public byte PricesIncludingVat { get; set; }

    public string InvoiceDiscCode { get; set; } = null!;

    public string CustomerDiscGroup { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public string SalespersonCode { get; set; } = null!;

    public string OrderNo { get; set; } = null!;

    public int NoPrinted { get; set; }

    public string OnHold { get; set; } = null!;

    public int AppliesToDocType { get; set; }

    public string AppliesToDocNo { get; set; } = null!;

    public string BalAccountNo { get; set; } = null!;

    public string VatRegistrationNo { get; set; } = null!;

    public string ReasonCode { get; set; } = null!;

    public string GenBusPostingGroup { get; set; } = null!;

    public byte Eu3PartyTrade { get; set; }

    public string TransactionType { get; set; } = null!;

    public string TransportMethod { get; set; } = null!;

    public string VatCountryRegionCode { get; set; } = null!;

    public string SellToCustomerName { get; set; } = null!;

    public string SellToCustomerName2 { get; set; } = null!;

    public string SellToAddress { get; set; } = null!;

    public string SellToAddress2 { get; set; } = null!;

    public string SellToCity { get; set; } = null!;

    public string SellToContact { get; set; } = null!;

    public string BillToPostCode { get; set; } = null!;

    public string BillToCounty { get; set; } = null!;

    public string BillToCountryRegionCode { get; set; } = null!;

    public string SellToPostCode { get; set; } = null!;

    public string SellToCounty { get; set; } = null!;

    public string SellToCountryRegionCode { get; set; } = null!;

    public string ShipToPostCode { get; set; } = null!;

    public string ShipToCounty { get; set; } = null!;

    public string ShipToCountryRegionCode { get; set; } = null!;

    public int BalAccountType { get; set; }

    public string ExitPoint { get; set; } = null!;

    public byte Correction { get; set; }

    public DateTime DocumentDate { get; set; }

    public string ExternalDocumentNo { get; set; } = null!;

    public string Area { get; set; } = null!;

    public string TransactionSpecification { get; set; } = null!;

    public string PaymentMethodCode { get; set; } = null!;

    public string ShippingAgentCode { get; set; } = null!;

    public string PackageTrackingNo { get; set; } = null!;

    public string NoSeries { get; set; } = null!;

    public string OrderNoSeries { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string SourceCode { get; set; } = null!;

    public string TaxAreaCode { get; set; } = null!;

    public byte TaxLiable { get; set; }

    public string VatBusPostingGroup { get; set; } = null!;

    public decimal VatBaseDiscount { get; set; }

    public string QuoteNo { get; set; } = null!;

    public int DimensionSetId { get; set; }

    public string CampaignNo { get; set; } = null!;

    public string SellToContactNo { get; set; } = null!;

    public string BillToContactNo { get; set; } = null!;

    public string ResponsibilityCenter { get; set; } = null!;

    public DateTime RequestedDeliveryDate { get; set; }

    public DateTime PromisedDeliveryDate { get; set; }

    public string ShippingTime { get; set; } = null!;

    public string OutboundWhseHandlingTime { get; set; } = null!;

    public string ShippingAgentServiceCode { get; set; } = null!;

    public byte AllowLineDisc { get; set; }

    public byte SpecialVatRateEnabled { get; set; }

    public decimal DocumentsStandardVatRate { get; set; }

    public decimal DocumentsSpecialVatRate { get; set; }

    public string ExchangeRateTypeCode { get; set; } = null!;

    public decimal ExchRateTypeCurrFactor { get; set; }

    public string StoreNo { get; set; } = null!;

    public int RetailStatus { get; set; }

    public string RecivingPickingNo { get; set; } = null!;

    public DateTime RingBackDate { get; set; }

    public string ShipToTelephone { get; set; } = null!;

    public byte RetailSpecialOrder { get; set; }

    public DateTime CustomerContactDate { get; set; }

    public int SpecialOrderOrigin { get; set; }

    public string DeliveryReferenceNo { get; set; } = null!;

    public byte SpoCreatedEntry { get; set; }

    public string BillToHouseNo { get; set; } = null!;

    public string ShipToHouseNo { get; set; } = null!;

    public DateTime EstimatedDeliveryDate { get; set; }

    public string PhoneNo { get; set; } = null!;

    public string DaytimePhoneNo { get; set; } = null!;

    public string MobilePhoneNo { get; set; } = null!;

    public string EMail { get; set; } = null!;

    public byte ContMail { get; set; }

    public byte ContPhone { get; set; }

    public byte ContEMail { get; set; }

    public string RetailZonesCode { get; set; } = null!;

    public string RetailZonesDescription { get; set; } = null!;

    public string StatementNo { get; set; } = null!;

    public string MemberCardNo { get; set; } = null!;
    
    public virtual ICollection<MikesportCoSALSalesShipmentLine> Lines { get; set; } = new List<MikesportCoSALSalesShipmentLine>();
}
