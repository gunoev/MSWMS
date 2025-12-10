using System;
using System.Collections.Generic;

namespace MSWMS.Entities.External;

public partial class MikesportCoSALTransferShipmentHeader
{
    public byte[] Timestamp { get; set; } = null!;

    public string No { get; set; } = null!;

    public string TransferFromCode { get; set; } = null!;

    public string TransferFromName { get; set; } = null!;

    public string TransferFromName2 { get; set; } = null!;

    public string TransferFromAddress { get; set; } = null!;

    public string TransferFromAddress2 { get; set; } = null!;

    public string TransferFromPostCode { get; set; } = null!;

    public string TransferFromCity { get; set; } = null!;

    public string TransferFromCounty { get; set; } = null!;

    public string TrsfFromCountryRegionCode { get; set; } = null!;

    public string TransferToCode { get; set; } = null!;

    public string TransferToName { get; set; } = null!;

    public string TransferToName2 { get; set; } = null!;

    public string TransferToAddress { get; set; } = null!;

    public string TransferToAddress2 { get; set; } = null!;

    public string TransferToPostCode { get; set; } = null!;

    public string TransferToCity { get; set; } = null!;

    public string TransferToCounty { get; set; } = null!;

    public string TrsfToCountryRegionCode { get; set; } = null!;

    public DateTime TransferOrderDate { get; set; }

    public DateTime PostingDate { get; set; }

    public string ShortcutDimension1Code { get; set; } = null!;

    public string ShortcutDimension2Code { get; set; } = null!;

    public string TransferOrderNo { get; set; } = null!;

    public string NoSeries { get; set; } = null!;

    public DateTime ShipmentDate { get; set; }

    public DateTime ReceiptDate { get; set; }

    public string InTransitCode { get; set; } = null!;

    public string TransferFromContact { get; set; } = null!;

    public string TransferToContact { get; set; } = null!;

    public string ExternalDocumentNo { get; set; } = null!;

    public string ShippingAgentCode { get; set; } = null!;

    public string ShippingAgentServiceCode { get; set; } = null!;

    public string ShipmentMethodCode { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public string TransportMethod { get; set; } = null!;

    public string EntryExitPoint { get; set; } = null!;

    public string Area { get; set; } = null!;

    public string TransactionSpecification { get; set; } = null!;

    public int DimensionSetId { get; set; }

    public string CreatedByUser { get; set; } = null!;

    public DateTime DateTimeCreated { get; set; }

    public string LastModifiedByUser { get; set; } = null!;

    public DateTime LastDateTimeModified { get; set; }

    public string PurchaseOrderNo { get; set; } = null!;

    public string StoreTo { get; set; } = null!;

    public string StoreFrom { get; set; } = null!;

    public int RetailStatus { get; set; }

    public string RecivingPickingNo { get; set; } = null!;
    
    public virtual ICollection<MikesportCoSALTransferShipmentLine> Lines { get; set; } = new List<MikesportCoSALTransferShipmentLine>();
}
