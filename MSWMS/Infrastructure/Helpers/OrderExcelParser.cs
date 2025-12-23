using ClosedXML.Excel;

namespace MSWMS.Infrastructure.Helpers;

public class OrderExcelParser
{
    public async Task<ExcelParsedOrder> Parse(string filePath)
    {
        using var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheet(1);
        
        var order = new ExcelParsedOrder
        {
            TransferOrderNumber = worksheet.Cell("X41").GetString(),
            TransferShipmentNumber = worksheet.Cell("J37").GetString(),
            Origin = worksheet.Cell("T7").GetString(),
            OriginCode = worksheet.Cell("X39").GetString(),
            Destination = worksheet.Cell("A8").GetString(),
            DestinationCode = worksheet.Cell("J39").GetString(),
            ShipmentId = Path.GetFileNameWithoutExtension(filePath)
        };

        var items = await ParseItems(worksheet);
        
        order.Items = items;

        return order;

    }

    public static async Task<string> GetShipmentNumber(string filePath)
    {
        using var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheet(1);
        
        return worksheet.Cell("J37").GetString();
    } 

    private async Task<List<ExcelParsedItem>> ParseItems(IXLWorksheet worksheet)
    {
        var rows = worksheet.Rows().Skip(55).ToList();
        
        var items = new List<ExcelParsedItem>();

        foreach (var row in rows.TakeWhile(row => row.Cell(11).GetString() != "Total Lines:"))
        {
            var item = await ParseItem(row);
            
            var existingItem = items.FirstOrDefault(i => i.Barcode == item.Barcode);
            
            if (existingItem != null)
            {
                existingItem.NeededQuantity += item.NeededQuantity;
            }
            else
            {
                items.Add(item);
            }
        }

        return items;
    }

    private Task<ExcelParsedItem> ParseItem(IXLRow row)
    {
        var item = new ExcelParsedItem
        {
            Barcode = row.Cell(2).GetString(),
            NeededQuantity = int.Parse(row.Cell(22).GetString()),
            ItemNumber = row.Cell("K").GetString(),
            Variant = row.Cell(13).GetString(),
            Description = row.Cell(17).GetString(),
        };

        return Task.FromResult(item);
    }
}