using System;
using System.Collections.Generic;

namespace MSWMS.Entities.External;

public partial class MikesportCoSALSalesLine
{
    public byte[] Timestamp { get; set; } = null!;

    public int DocumentType { get; set; }

    public string DocumentNo { get; set; } = null!;

    public int LineNo { get; set; }

    public string SellToCustomerNo { get; set; } = null!;

    public int Type { get; set; }

    public string No { get; set; } = null!;

    public string LocationCode { get; set; } = null!;

    public string PostingGroup { get; set; } = null!;

    public DateTime ShipmentDate { get; set; }

    public string Description { get; set; } = null!;

    public string Description2 { get; set; } = null!;

    public string UnitOfMeasure { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal OutstandingQuantity { get; set; }

    public decimal QtyToInvoice { get; set; }

    public decimal QtyToShip { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal UnitCostLcy { get; set; }

    public decimal Vat { get; set; }

    public decimal LineDiscount { get; set; }

    public decimal LineDiscountAmount { get; set; }

    public decimal Amount { get; set; }

    public decimal AmountIncludingVat { get; set; }

    public byte AllowInvoiceDisc { get; set; }

    public decimal GrossWeight { get; set; }

    public decimal NetWeight { get; set; }

    public decimal UnitsPerParcel { get; set; }

    public decimal UnitVolume { get; set; }

    public int ApplToItemEntry { get; set; }

    public string ShortcutDimension1Code { get; set; } = null!;

    public string ShortcutDimension2Code { get; set; } = null!;

    public string CustomerPriceGroup { get; set; } = null!;

    public string JobNo { get; set; } = null!;

    public string WorkTypeCode { get; set; } = null!;

    public decimal OutstandingAmount { get; set; }

    public decimal QtyShippedNotInvoiced { get; set; }

    public decimal ShippedNotInvoiced { get; set; }

    public decimal QuantityShipped { get; set; }

    public decimal QuantityInvoiced { get; set; }

    public string ShipmentNo { get; set; } = null!;

    public int ShipmentLineNo { get; set; }

    public decimal Profit { get; set; }

    public string BillToCustomerNo { get; set; } = null!;

    public decimal InvDiscountAmount { get; set; }

    public string PurchaseOrderNo { get; set; } = null!;

    public int PurchOrderLineNo { get; set; }

    public byte DropShipment { get; set; }

    public string GenBusPostingGroup { get; set; } = null!;

    public string GenProdPostingGroup { get; set; } = null!;

    public int VatCalculationType { get; set; }

    public string TransactionType { get; set; } = null!;

    public string TransportMethod { get; set; } = null!;

    public int AttachedToLineNo { get; set; }

    public string ExitPoint { get; set; } = null!;

    public string Area { get; set; } = null!;

    public string TransactionSpecification { get; set; } = null!;

    public string TaxAreaCode { get; set; } = null!;

    public byte TaxLiable { get; set; }

    public string TaxGroupCode { get; set; } = null!;

    public string VatClauseCode { get; set; } = null!;

    public string VatBusPostingGroup { get; set; } = null!;

    public string VatProdPostingGroup { get; set; } = null!;

    public string CurrencyCode { get; set; } = null!;

    public decimal OutstandingAmountLcy { get; set; }

    public decimal ShippedNotInvoicedLcy { get; set; }

    public int Reserve { get; set; }

    public string BlanketOrderNo { get; set; } = null!;

    public int BlanketOrderLineNo { get; set; }

    public decimal VatBaseAmount { get; set; }

    public decimal UnitCost { get; set; }

    public byte SystemCreatedEntry { get; set; }

    public decimal LineAmount { get; set; }

    public decimal VatDifference { get; set; }

    public decimal InvDiscAmountToInvoice { get; set; }

    public string VatIdentifier { get; set; } = null!;

    public int IcPartnerRefType { get; set; }

    public string IcPartnerReference { get; set; } = null!;

    public decimal Prepayment { get; set; }

    public decimal PrepmtLineAmount { get; set; }

    public decimal PrepmtAmtInv { get; set; }

    public decimal PrepmtAmtInclVat { get; set; }

    public decimal PrepaymentAmount { get; set; }

    public decimal PrepmtVatBaseAmt { get; set; }

    public decimal PrepaymentVat { get; set; }

    public int PrepmtVatCalcType { get; set; }

    public string PrepaymentVatIdentifier { get; set; } = null!;

    public string PrepaymentTaxAreaCode { get; set; } = null!;

    public byte PrepaymentTaxLiable { get; set; }

    public string PrepaymentTaxGroupCode { get; set; } = null!;

    public decimal PrepmtAmtToDeduct { get; set; }

    public decimal PrepmtAmtDeducted { get; set; }

    public byte PrepaymentLine { get; set; }

    public decimal PrepmtAmountInvInclVat { get; set; }

    public decimal PrepmtAmountInvLcy { get; set; }

    public string IcPartnerCode { get; set; } = null!;

    public decimal PrepmtVatAmountInvLcy { get; set; }

    public decimal PrepaymentVatDifference { get; set; }

    public decimal PrepmtVatDiffToDeduct { get; set; }

    public decimal PrepmtVatDiffDeducted { get; set; }

    public int DimensionSetId { get; set; }

    public decimal QtyToAssembleToOrder { get; set; }

    public decimal QtyToAsmToOrderBase { get; set; }

    public string JobTaskNo { get; set; } = null!;

    public int JobContractEntryNo { get; set; }

    public string VariantCode { get; set; } = null!;

    public string BinCode { get; set; } = null!;

    public decimal QtyPerUnitOfMeasure { get; set; }

    public byte Planned { get; set; }

    public string UnitOfMeasureCode { get; set; } = null!;

    public decimal QuantityBase { get; set; }

    public decimal OutstandingQtyBase { get; set; }

    public decimal QtyToInvoiceBase { get; set; }

    public decimal QtyToShipBase { get; set; }

    public decimal QtyShippedNotInvdBase { get; set; }

    public decimal QtyShippedBase { get; set; }

    public decimal QtyInvoicedBase { get; set; }

    public DateTime FaPostingDate { get; set; }

    public string DepreciationBookCode { get; set; } = null!;

    public byte DeprUntilFaPostingDate { get; set; }

    public string DuplicateInDepreciationBook { get; set; } = null!;

    public byte UseDuplicationList { get; set; }

    public string ResponsibilityCenter { get; set; } = null!;

    public byte OutOfStockSubstitution { get; set; }

    public string OriginallyOrderedNo { get; set; } = null!;

    public string OriginallyOrderedVarCode { get; set; } = null!;

    public string CrossReferenceNo { get; set; } = null!;

    public string UnitOfMeasureCrossRef { get; set; } = null!;

    public int CrossReferenceType { get; set; }

    public string CrossReferenceTypeNo { get; set; } = null!;

    public string ItemCategoryCode { get; set; } = null!;

    public byte Nonstock { get; set; }

    public string PurchasingCode { get; set; } = null!;

    public string ProductGroupCode { get; set; } = null!;

    public byte SpecialOrder { get; set; }

    public string SpecialOrderPurchaseNo { get; set; } = null!;

    public int SpecialOrderPurchLineNo { get; set; }

    public byte CompletelyShipped { get; set; }

    public DateTime RequestedDeliveryDate { get; set; }

    public DateTime PromisedDeliveryDate { get; set; }

    public string ShippingTime { get; set; } = null!;

    public string OutboundWhseHandlingTime { get; set; } = null!;

    public DateTime PlannedDeliveryDate { get; set; }

    public DateTime PlannedShipmentDate { get; set; }

    public string ShippingAgentCode { get; set; } = null!;

    public string ShippingAgentServiceCode { get; set; } = null!;

    public byte AllowItemChargeAssignment { get; set; }

    public decimal ReturnQtyToReceive { get; set; }

    public decimal ReturnQtyToReceiveBase { get; set; }

    public decimal ReturnQtyRcdNotInvd { get; set; }

    public decimal RetQtyRcdNotInvdBase { get; set; }

    public decimal ReturnRcdNotInvd { get; set; }

    public decimal ReturnRcdNotInvdLcy { get; set; }

    public decimal ReturnQtyReceived { get; set; }

    public decimal ReturnQtyReceivedBase { get; set; }

    public int ApplFromItemEntry { get; set; }

    public string BomItemNo { get; set; } = null!;

    public string ReturnReceiptNo { get; set; } = null!;

    public int ReturnReceiptLineNo { get; set; }

    public string ReturnReasonCode { get; set; } = null!;

    public byte AllowLineDisc { get; set; }

    public string CustomerDiscGroup { get; set; } = null!;

    public string BarcodeNo { get; set; } = null!;

    public string ItemFamilyCode { get; set; } = null!;

    public string ItemBrandCode { get; set; } = null!;

    public string PriceRangeCode { get; set; } = null!;

    public string ItemGenderLevel1Code { get; set; } = null!;

    public string ItemGenderLevel2Code { get; set; } = null!;

    public string ItemLineCode { get; set; } = null!;

    public string SeasonCode { get; set; } = null!;

    public byte IsConsigned { get; set; }

    public string WarrantyPeriod { get; set; } = null!;

    public byte ExcludeZeroPricePrinting { get; set; }

    public string AutoAccGroup { get; set; } = null!;

    public decimal VatAmntInOriginalCurrency { get; set; }

    public decimal VatAmountInLcy { get; set; }

    public decimal AmountIncludingVatLcy { get; set; }

    public int ReplicationCounter { get; set; }

    public byte SendToWms { get; set; }

    public int WmsStatus { get; set; }

    public string StoreNo { get; set; } = null!;

    public string CurrentCustPriceGroup { get; set; } = null!;

    public string CurrentStoreGroup { get; set; } = null!;

    public string Division { get; set; } = null!;

    public string OfferNo { get; set; } = null!;

    public string PromotionNo { get; set; } = null!;

    public string AllocPlanPurcOrderNo { get; set; } = null!;

    public byte RetailSpecialOrder { get; set; }

    public int DeliveringMethod { get; set; }

    public int VendorDeliversTo { get; set; }

    public int Sourcing { get; set; }

    public int DeliverFrom { get; set; }

    public string DeliveryLocationCode { get; set; } = null!;

    public decimal SpoPrepayment { get; set; }

    public int WhseProcess { get; set; }

    public int DeliveryStatus { get; set; }

    public string ConfigurationId { get; set; } = null!;

    public int AddChargeOption { get; set; }

    public string DeliveryReferenceNo { get; set; } = null!;

    public string DeliveryCancelUserId { get; set; } = null!;

    public DateTime DeliveryCancelDateTime { get; set; }

    public decimal Counter { get; set; }

    public string OptionValueText { get; set; } = null!;

    public DateTime EstimatedDeliveryDate { get; set; }

    public DateTime NoLaterThanDate { get; set; }

    public decimal PaymentAtOrderEntryLimit { get; set; }

    public decimal PaymentAtDeliveryLimit { get; set; }

    public int ReturnPolicy { get; set; }

    public decimal NonRefundAmount { get; set; }

    public int SourcingStatus { get; set; }

    public decimal PaymentAtPurchaseOrderLimit { get; set; }

    public int SpoDocumentMethod { get; set; }

    public string StoreSalesLocation { get; set; } = null!;

    public string SpoWhseLocation { get; set; } = null!;

    public string VendorNo { get; set; } = null!;

    public string ItemTrackingNo { get; set; } = null!;

    public string PaymentProfileCode { get; set; } = null!;

    public string CreatedAtPosTermNo { get; set; } = null!;

    public byte ErrorInProcess { get; set; }

    public int CancelPermitted { get; set; }

    public int LinkedToLineNo { get; set; }

    public string AddChargeCode { get; set; } = null!;

    public string ReservedByPosNo { get; set; } = null!;
}
