using System;
using System.Collections.Generic;

namespace MSWMS.TempModels;

public partial class DcxMsWarehouseActivityHeader
{
    public byte[] Timestamp { get; set; } = null!;

    public int Type { get; set; }

    public string No { get; set; } = null!;

    public string LocationCode { get; set; } = null!;

    public string AssignedUserId { get; set; } = null!;

    public DateTime AssignmentDate { get; set; }

    public DateTime AssignmentTime { get; set; }

    public int SortingMethod { get; set; }

    public string NoSeries { get; set; } = null!;

    public int NoPrinted { get; set; }

    public DateTime PostingDate { get; set; }

    public string RegisteringNo { get; set; } = null!;

    public string LastRegisteringNo { get; set; } = null!;

    public string RegisteringNoSeries { get; set; } = null!;

    public DateTime DateOfLastPrinting { get; set; }

    public DateTime TimeOfLastPrinting { get; set; }

    public byte BreakbulkFilter { get; set; }

    public string SourceNo { get; set; } = null!;

    public int SourceDocument { get; set; }

    public int SourceType { get; set; }

    public int SourceSubtype { get; set; }

    public int DestinationType { get; set; }

    public string DestinationNo { get; set; } = null!;

    public string ExternalDocumentNo { get; set; } = null!;

    public DateTime ExpectedReceiptDate { get; set; }

    public DateTime ShipmentDate { get; set; }

    public string ExternalDocumentNo2 { get; set; } = null!;
}
