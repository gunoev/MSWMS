using System;
using System.Collections.Generic;

namespace MSWMS.Entities.External;

public partial class MikesportCoSALSalesHeader
{
    public byte[] Timestamp { get; set; } = null!;

    public int DocumentType { get; set; }

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

    public string OrderClass { get; set; } = null!;

    public int NoPrinted { get; set; }

    public string OnHold { get; set; } = null!;

    public int AppliesToDocType { get; set; }

    public string AppliesToDocNo { get; set; } = null!;

    public string BalAccountNo { get; set; } = null!;

    public byte Ship { get; set; }

    public byte Invoice { get; set; }

    public byte PrintPostedDocuments { get; set; }

    public string ShippingNo { get; set; } = null!;

    public string PostingNo { get; set; } = null!;

    public string LastShippingNo { get; set; } = null!;

    public string LastPostingNo { get; set; } = null!;

    public string PrepaymentNo { get; set; } = null!;

    public string LastPrepaymentNo { get; set; } = null!;

    public string PrepmtCrMemoNo { get; set; } = null!;

    public string LastPrepmtCrMemoNo { get; set; } = null!;

    public string VatRegistrationNo { get; set; } = null!;

    public byte CombineShipments { get; set; }

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

    public string PostingNoSeries { get; set; } = null!;

    public string ShippingNoSeries { get; set; } = null!;

    public string TaxAreaCode { get; set; } = null!;

    public byte TaxLiable { get; set; }

    public string VatBusPostingGroup { get; set; } = null!;

    public int Reserve { get; set; }

    public string AppliesToId { get; set; } = null!;

    public decimal VatBaseDiscount { get; set; }

    public int Status { get; set; }

    public int InvoiceDiscountCalculation { get; set; }

    public decimal InvoiceDiscountValue { get; set; }

    public byte SendIcDocument { get; set; }

    public int IcStatus { get; set; }

    public string SellToIcPartnerCode { get; set; } = null!;

    public string BillToIcPartnerCode { get; set; } = null!;

    public int IcDirection { get; set; }

    public decimal Prepayment { get; set; }

    public string PrepaymentNoSeries { get; set; } = null!;

    public byte CompressPrepayment { get; set; }

    public DateTime PrepaymentDueDate { get; set; }

    public string PrepmtCrMemoNoSeries { get; set; } = null!;

    public string PrepmtPostingDescription { get; set; } = null!;

    public DateTime PrepmtPmtDiscountDate { get; set; }

    public string PrepmtPaymentTermsCode { get; set; } = null!;

    public decimal PrepmtPaymentDiscount { get; set; }

    public string QuoteNo { get; set; } = null!;

    public int JobQueueStatus { get; set; }

    public Guid JobQueueEntryId { get; set; }

    public int IncomingDocumentEntryNo { get; set; }

    public int DimensionSetId { get; set; }

    public byte AuthorizationRequired { get; set; }

    public string CreditCardNo { get; set; } = null!;

    public string DirectDebitMandateId { get; set; } = null!;

    public int DocNoOccurrence { get; set; }

    public string CampaignNo { get; set; } = null!;

    public string SellToCustomerTemplateCode { get; set; } = null!;

    public string SellToContactNo { get; set; } = null!;

    public string BillToContactNo { get; set; } = null!;

    public string BillToCustomerTemplateCode { get; set; } = null!;

    public string OpportunityNo { get; set; } = null!;

    public string ResponsibilityCenter { get; set; } = null!;

    public int ShippingAdvice { get; set; }

    public int PostingFromWhseRef { get; set; }

    public DateTime RequestedDeliveryDate { get; set; }

    public DateTime PromisedDeliveryDate { get; set; }

    public string ShippingTime { get; set; } = null!;

    public string OutboundWhseHandlingTime { get; set; } = null!;

    public string ShippingAgentServiceCode { get; set; } = null!;

    public byte Receive { get; set; }

    public string ReturnReceiptNo { get; set; } = null!;

    public string ReturnReceiptNoSeries { get; set; } = null!;

    public string LastReturnReceiptNo { get; set; } = null!;

    public byte AllowLineDisc { get; set; }

    public byte GetShipmentUsed { get; set; }

    public string AssignedUserId { get; set; } = null!;

    public DateTime ValidityDate { get; set; }

    public string CreatedByUser { get; set; } = null!;

    public DateTime DateTimeCreated { get; set; }

    public string LastModifiedByUser { get; set; } = null!;

    public DateTime LastDateTimeModified { get; set; }

    public string StoreCommission { get; set; } = null!;

    public byte SpecialVatRateEnabled { get; set; }

    public decimal DocumentsStandardVatRate { get; set; }

    public decimal DocumentsSpecialVatRate { get; set; }

    public string ExchangeRateTypeCode { get; set; } = null!;

    public decimal ExchRateTypeCurrFactor { get; set; }

    public int ReplicationCounter { get; set; }

    public byte SendToWms { get; set; }

    public int IntegrationStatus { get; set; }

    public string BatchNo { get; set; } = null!;

    public string StoreNo { get; set; } = null!;

    public string PosId { get; set; } = null!;

    public DateTime PostingTime { get; set; }

    public string CashierId { get; set; } = null!;

    public byte NotShowDialog { get; set; }

    public string SalesType { get; set; } = null!;

    public string PosComment { get; set; } = null!;

    public int RetailStatus { get; set; }

    public string ReceivingPickingNo { get; set; } = null!;

    public string XRetailLocationCode { get; set; } = null!;

    public DateTime OwnershipSendDate { get; set; }

    public byte InStoreCreatedEntry { get; set; }

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

    public string SourceCode { get; set; } = null!;

    public string RetailZonesCode { get; set; } = null!;

    public string StatementNo { get; set; } = null!;

    public string MemberCardNo { get; set; } = null!;
}
