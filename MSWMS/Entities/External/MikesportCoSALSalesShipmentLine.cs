using System;
using System.Collections.Generic;

namespace MSWMS.Entities.External;

public partial class MikesportCoSALSalesShipmentLine
{
    public byte[] Timestamp { get; set; } = null!;

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

    public decimal UnitPrice { get; set; }

    public decimal UnitCostLcy { get; set; }

    public decimal Vat { get; set; }

    public decimal LineDiscount { get; set; }

    public byte AllowInvoiceDisc { get; set; }

    public decimal GrossWeight { get; set; }

    public decimal NetWeight { get; set; }

    public decimal UnitsPerParcel { get; set; }

    public decimal UnitVolume { get; set; }

    public int ApplToItemEntry { get; set; }

    public int ItemShptEntryNo { get; set; }

    public string ShortcutDimension1Code { get; set; } = null!;

    public string ShortcutDimension2Code { get; set; } = null!;

    public string CustomerPriceGroup { get; set; } = null!;

    public string JobNo { get; set; } = null!;

    public string WorkTypeCode { get; set; } = null!;

    public decimal QtyShippedNotInvoiced { get; set; }

    public decimal QuantityInvoiced { get; set; }

    public string OrderNo { get; set; } = null!;

    public int OrderLineNo { get; set; }

    public string BillToCustomerNo { get; set; } = null!;

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

    public string VatBusPostingGroup { get; set; } = null!;

    public string VatProdPostingGroup { get; set; } = null!;

    public string BlanketOrderNo { get; set; } = null!;

    public int BlanketOrderLineNo { get; set; }

    public decimal VatBaseAmount { get; set; }

    public decimal UnitCost { get; set; }

    public DateTime PostingDate { get; set; }

    public int DimensionSetId { get; set; }

    public byte AuthorizedForCreditCard { get; set; }

    public string JobTaskNo { get; set; } = null!;

    public int JobContractEntryNo { get; set; }

    public string VariantCode { get; set; } = null!;

    public string BinCode { get; set; } = null!;

    public decimal QtyPerUnitOfMeasure { get; set; }

    public string UnitOfMeasureCode { get; set; } = null!;

    public decimal QuantityBase { get; set; }

    public decimal QtyInvoicedBase { get; set; }

    public DateTime FaPostingDate { get; set; }

    public string DepreciationBookCode { get; set; } = null!;

    public byte DeprUntilFaPostingDate { get; set; }

    public string DuplicateInDepreciationBook { get; set; } = null!;

    public byte UseDuplicationList { get; set; }

    public string ResponsibilityCenter { get; set; } = null!;

    public string CrossReferenceNo { get; set; } = null!;

    public string UnitOfMeasureCrossRef { get; set; } = null!;

    public int CrossReferenceType { get; set; }

    public string CrossReferenceTypeNo { get; set; } = null!;

    public string ItemCategoryCode { get; set; } = null!;

    public byte Nonstock { get; set; }

    public string PurchasingCode { get; set; } = null!;

    public string ProductGroupCode { get; set; } = null!;

    public DateTime RequestedDeliveryDate { get; set; }

    public DateTime PromisedDeliveryDate { get; set; }

    public string ShippingTime { get; set; } = null!;

    public string OutboundWhseHandlingTime { get; set; } = null!;

    public DateTime PlannedDeliveryDate { get; set; }

    public DateTime PlannedShipmentDate { get; set; }

    public int ApplFromItemEntry { get; set; }

    public decimal ItemChargeBaseAmount { get; set; }

    public byte Correction { get; set; }

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

    public string DivisionCode { get; set; } = null!;

    public byte RetailSpecialOrder { get; set; }

    public int DeliveringMethod { get; set; }

    public int VendorDeliversTo { get; set; }

    public int Sourcing { get; set; }

    public int DeliverFrom { get; set; }

    public string DeliveryLocationCode { get; set; } = null!;

    public string ConfigurationId { get; set; } = null!;

    public string DeliveryReferenceNo { get; set; } = null!;

    public string DeliveryUserId { get; set; } = null!;

    public DateTime DeliveryDateTime { get; set; }

    public string OptionValueText { get; set; } = null!;

    public DateTime EstimatedDeliveryDate { get; set; }

    public DateTime NoLaterThanDate { get; set; }

    public int ReturnPolicy { get; set; }

    public int SpoDocumentMethod { get; set; }

    public string StoreSalesLocation { get; set; } = null!;

    public string SpoWhseLocation { get; set; } = null!;

    public string VendorNo { get; set; } = null!;

    public string ItemTrackingNo { get; set; } = null!;
    
    public virtual MikesportCoSALSalesShipmentHeader Header { get; set; } = null!;
}
