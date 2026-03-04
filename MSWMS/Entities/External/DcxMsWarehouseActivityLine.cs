using System;
using System.Collections.Generic;

namespace MSWMS.TempModels;

public partial class DcxMsWarehouseActivityLine
{
    public byte[] Timestamp { get; set; } = null!;

    public int ActivityType { get; set; }

    public string No { get; set; } = null!;

    public int LineNo { get; set; }

    public int SourceType { get; set; }

    public int SourceSubtype { get; set; }

    public string SourceNo { get; set; } = null!;

    public int SourceLineNo { get; set; }

    public int SourceSublineNo { get; set; }

    public int SourceDocument { get; set; }

    public string LocationCode { get; set; } = null!;

    public string ShelfNo { get; set; } = null!;

    public int SortingSequenceNo { get; set; }

    public string ItemNo { get; set; } = null!;

    public string VariantCode { get; set; } = null!;

    public string UnitOfMeasureCode { get; set; } = null!;

    public decimal QtyPerUnitOfMeasure { get; set; }

    public string Description { get; set; } = null!;

    public string Description2 { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal QtyBase { get; set; }

    public decimal QtyOutstanding { get; set; }

    public decimal QtyOutstandingBase { get; set; }

    public decimal QtyToHandle { get; set; }

    public decimal QtyToHandleBase { get; set; }

    public decimal QtyHandled { get; set; }

    public decimal QtyHandledBase { get; set; }

    public int ShippingAdvice { get; set; }

    public DateTime DueDate { get; set; }

    public int DestinationType { get; set; }

    public string DestinationNo { get; set; } = null!;

    public string ShippingAgentCode { get; set; } = null!;

    public string ShippingAgentServiceCode { get; set; } = null!;

    public string ShipmentMethodCode { get; set; } = null!;

    public DateTime StartingDate { get; set; }

    public byte AssembleToOrder { get; set; }

    public byte AtoComponent { get; set; }

    public string SerialNo { get; set; } = null!;

    public string LotNo { get; set; } = null!;

    public DateTime WarrantyDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string BinCode { get; set; } = null!;

    public string ZoneCode { get; set; } = null!;

    public int ActionType { get; set; }

    public int WhseDocumentType { get; set; }

    public string WhseDocumentNo { get; set; } = null!;

    public int WhseDocumentLineNo { get; set; }

    public int BinRanking { get; set; }

    public decimal Cubage { get; set; }

    public decimal Weight { get; set; }

    public string SpecialEquipmentCode { get; set; } = null!;

    public string BinTypeCode { get; set; } = null!;

    public int BreakbulkNo { get; set; }

    public byte OriginalBreakbulk { get; set; }

    public byte Breakbulk { get; set; }

    public int CrossDockInformation { get; set; }

    public byte Dedicated { get; set; }

    public string ShippingId { get; set; } = null!;
}
