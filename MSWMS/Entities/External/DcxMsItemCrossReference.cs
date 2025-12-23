using System;
using System.Collections.Generic;

namespace MSWMS.Models;

public partial class DcxMsItemCrossReference
{
    public byte[] Timestamp { get; set; } = null!;

    public string ItemNo { get; set; } = null!;

    public string VariantCode { get; set; } = null!;

    public string UnitOfMeasure { get; set; } = null!;

    public int CrossReferenceType { get; set; }

    public string CrossReferenceTypeNo { get; set; } = null!;

    public string CrossReferenceNo { get; set; } = null!;

    public string Description { get; set; } = null!;

    public byte DiscontinueBarCode { get; set; }
}
