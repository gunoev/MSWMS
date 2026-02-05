using System;
using System.Collections.Generic;

namespace MSWMS.TempModels;

public partial class MikesportCoSALDefaultSalesPrice
{
    public byte[] Timestamp { get; set; } = null!;

    public string ItemNo { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string UnitOfMeasureCode { get; set; } = null!;

    public decimal DiscountedPrice { get; set; }

    public decimal DiscountPercentage { get; set; }

    public int ReplicationCounter { get; set; }

    public string Currency { get; set; } = null!;

    public decimal UnitPriceIncludingVatCurre { get; set; }

    public decimal DiscountedPriceCurrency { get; set; }

    public decimal UnitPriceIncludingVat { get; set; }
}
