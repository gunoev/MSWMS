using System;
using System.Collections.Generic;

namespace MSWMS.Entities.External;

public partial class MikesportCoSALTransferLine
{
    public byte[] Timestamp { get; set; } = null!;

    public string DocumentNo { get; set; } = null!;

    public int LineNo { get; set; }

    public string ItemNo { get; set; } = null!;

    public decimal Quantity { get; set; }

    public string UnitOfMeasure { get; set; } = null!;

    public decimal QtyToShip { get; set; }

    public decimal QtyToReceive { get; set; }

    public decimal QuantityShipped { get; set; }

    public decimal QuantityReceived { get; set; }

    public int Status { get; set; }

    public string ShortcutDimension1Code { get; set; } = null!;

    public string ShortcutDimension2Code { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string GenProdPostingGroup { get; set; } = null!;

    public string InventoryPostingGroup { get; set; } = null!;

    public decimal QuantityBase { get; set; }

    public decimal OutstandingQtyBase { get; set; }

    public decimal QtyToShipBase { get; set; }

    public decimal QtyShippedBase { get; set; }

    public decimal QtyToReceiveBase { get; set; }

    public decimal QtyReceivedBase { get; set; }

    public decimal QtyPerUnitOfMeasure { get; set; }

    public string UnitOfMeasureCode { get; set; } = null!;

    public decimal OutstandingQuantity { get; set; }

    public decimal GrossWeight { get; set; }

    public decimal NetWeight { get; set; }

    public decimal UnitVolume { get; set; }

    public string VariantCode { get; set; } = null!;

    public decimal UnitsPerParcel { get; set; }

    public string Description2 { get; set; } = null!;

    public string InTransitCode { get; set; } = null!;

    public decimal QtyInTransit { get; set; }

    public decimal QtyInTransitBase { get; set; }

    public string TransferFromCode { get; set; } = null!;

    public string TransferToCode { get; set; } = null!;

    public DateTime ShipmentDate { get; set; }

    public DateTime ReceiptDate { get; set; }

    public int DerivedFromLineNo { get; set; }

    public string ShippingAgentCode { get; set; } = null!;

    public string ShippingAgentServiceCode { get; set; } = null!;

    public string ShippingTime { get; set; } = null!;

    public int DimensionSetId { get; set; }

    public string ItemCategoryCode { get; set; } = null!;

    public string ProductGroupCode { get; set; } = null!;

    public byte CompletelyShipped { get; set; }

    public byte CompletelyReceived { get; set; }

    public string OutboundWhseHandlingTime { get; set; } = null!;

    public string InboundWhseHandlingTime { get; set; } = null!;

    public string TransferFromBinCode { get; set; } = null!;

    public string TransferToBinCode { get; set; } = null!;

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

    public int ReplicationCounter { get; set; }

    public byte SendToWms { get; set; }

    public string ReceivingTransFromCode { get; set; } = null!;

    public string ReceivingTransToCode { get; set; } = null!;

    public string Division { get; set; } = null!;

    public decimal ActualQtyToReceive { get; set; }

    public decimal ActualQtyToReceiveBase { get; set; }

    public decimal QtyDifference { get; set; }

    public decimal QtyDifferenceBase { get; set; }

    public int InStoreDocumentStatus { get; set; }

    public string ReasonCode { get; set; } = null!;

    public int TransferType { get; set; }

    public string PurchaseOrderNo { get; set; } = null!;

    public string ConfigurationId { get; set; } = null!;

    public int PlanningFlexibility { get; set; }
}
