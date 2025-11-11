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

                if (itemInfos.Count >= batchSize)
                {
                    context.ItemInfos.AddRange(itemInfos);
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                    itemInfos.Clear();

                }
            }

            if (itemInfos.Count > 0)
            {
                context.ItemInfos.AddRange(itemInfos);
                context.SaveChanges();
            }
        }
    }
    
        public void ParseAndUpdate(string filePath)
    {
        var itemInfos = new List<ItemInfo>();
        int batchSize = 5000;
        int skippedCount = 0;

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
            var existingBarcodesInDb = context.ItemInfos
                .Select(i => i.Barcode)
                .ToHashSet();
            
            var processedInCurrentSession = new HashSet<string>();

            while (csv.Read())
            {
                var barcode = RemoveSpecialCharacters(csv.GetField(0) ?? string.Empty);

                if (processedInCurrentSession.Contains(barcode))
                {
                    skippedCount++;
                    continue;
                }
                
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
                processedInCurrentSession.Add(barcode);

                if (itemInfos.Count >= batchSize)
                {
                    UpdateBatch(context, itemInfos, existingBarcodesInDb);

                    foreach (var item in itemInfos)
                    {
                        existingBarcodesInDb.Add(item.Barcode);
                    }
                    
                    itemInfos.Clear();

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            if (itemInfos.Count > 0)
            {
                UpdateBatch(context, itemInfos, existingBarcodesInDb);
            }
            
            if (skippedCount > 0)
            {
                Console.WriteLine($"Пропущено дубликатов: {skippedCount}");
            }
        }
    }

    private void UpdateBatch(AppDbContext context, List<ItemInfo> itemInfos, HashSet<string> existingBarcodesInDb)
    {
        var barcodesToUpdate = itemInfos
            .Where(i => existingBarcodesInDb.Contains(i.Barcode))
            .Select(i => i.Barcode)
            .ToList();

        var existingItems = barcodesToUpdate.Any()
            ? context.ItemInfos
                .Where(i => barcodesToUpdate.Contains(i.Barcode))
                .ToDictionary(i => i.Barcode)
            : new Dictionary<string, ItemInfo>();

        foreach (var itemInfo in itemInfos)
        {
            if (existingItems.TryGetValue(itemInfo.Barcode, out var existingItem))
            {
                existingItem.ItemNumber = itemInfo.ItemNumber;
                existingItem.Variant = itemInfo.Variant;
                existingItem.Description = itemInfo.Description;
                existingItem.Price = itemInfo.Price;
                existingItem.UnitOfMeasure = itemInfo.UnitOfMeasure;
                existingItem.Na = itemInfo.Na;
                existingItem.DiscountPercentage = itemInfo.DiscountPercentage;
                existingItem.DiscountPrice = itemInfo.DiscountPrice;
                existingItem.Currency = itemInfo.Currency;
            }
            else
            {
                context.ItemInfos.Add(itemInfo);
            }
        }

        context.SaveChanges();
        context.ChangeTracker.Clear();
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