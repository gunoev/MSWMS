using System.Globalization;
using MSWMS.Entities;
using CsvHelper.Configuration;
using CsvReader = CsvHelper.CsvReader;

namespace MSWMS.Infrastructure.Helpers;

public class ItemInfoParser
{
    public void Parse(string filePath)
    {
        var itemInfos = new List<ItemInfo>();
        int batchSize = 5000;

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            MissingFieldFound = null,
            BadDataFound = null,
        };

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, config))
        using (var context = new AppDbContext())
        {
            while (csv.Read())
            {
                var barcode = RemoveSpecialCharacters(csv.GetField(0) ?? string.Empty);
                var itemNumber = csv.GetField(1);
                var variant = csv.GetField(2);
                var description = csv.GetField(3);
                var price = decimal.TryParse(csv.GetField(4)?.Replace(',', '.') ?? string.Empty, NumberStyles.Float, CultureInfo.InvariantCulture, out var priceParsed) ? priceParsed : 0.0m;
                var unitOfMeasure = csv.GetField(5);
                var na = csv.GetField(6);
                var discountPercentage = decimal.TryParse(csv.GetField(7), out var discountPerc) ? discountPerc : 0.0m;
                var discountPrice = decimal.TryParse(csv.GetField(8)?.Replace(',', '.') ?? string.Empty, NumberStyles.Float, CultureInfo.InvariantCulture, out var discPrice) ? discPrice : 0.0m;
                var currency = csv.GetField(9);

                var itemInfo = new ItemInfo
                {
                    Barcode = barcode,
                    ItemNumber = itemNumber ?? "",
                    Variant = variant,
                    Description = description,
                    Price = price,
                    UnitOfMeasure = unitOfMeasure,
                    Na = na,
                    DiscountPercentage = discountPercentage,
                    DiscountPrice = discountPrice,
                    Currency = currency
                };

                itemInfos.Add(itemInfo);

                //Console.WriteLine($"Processed {itemInfos.Count} records");

                // Если достигли размера пакета, выполняем вставку
                if (itemInfos.Count >= batchSize)
                {
                    context.ItemInfos.AddRange(itemInfos);
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                    itemInfos.Clear();

                }
            }

            // Вставляем оставшиеся записи
            if (itemInfos.Count > 0)
            {
                context.ItemInfos.AddRange(itemInfos);
                context.SaveChanges();
            }
        }
    }
    
    public static string PrepareBarcode(string input)
    {
        return input?.Trim() ?? string.Empty;
    }
        
    public static string RemoveSpecialCharacters(string input)
    {
        input = PrepareBarcode(input);
        return input.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(" ", string.Empty);
    }
}