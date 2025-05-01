using Application.Files.Interfaces;
using Domain.Constants;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Infrastructure.Files;

public class FileParser : IFileParser
{
    public char GetDelimiterFromCsv(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        string? headerLine = reader.ReadLine();

        if (string.IsNullOrWhiteSpace(headerLine))
        {
            throw new InvalidDataException("CSV file is empty or invalid.");
        }

        var counts = Delimiters.Allowed
            .Select(delimiter => new { Delimiter = delimiter, Count = headerLine.Count(c => c == delimiter) })
            .OrderByDescending(x => x.Count)
            .ToList();

        if (counts.First().Count == 0 || (counts.Count > 1 && counts[0].Count == counts[1].Count))
        {
            throw new InvalidDataException("Unable to determine delimiter.");
        }

        return counts.First().Delimiter;
    }

    public List<Product> GetProductsFromCsv(IFormFile file, char delimiter)
    {
        var products = new List<Product>();
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        using var parser = new TextFieldParser(reader)
        {
            TextFieldType = FieldType.Delimited
        };

        parser.SetDelimiters(delimiter.ToString());

        if (parser.EndOfData)
            throw new InvalidDataException("CSV file is empty.");

        // Read header and determine column positions
        string[] headers = parser.ReadFields();
        if (headers == null || headers.Length == 0)
            throw new InvalidDataException("CSV header is missing or invalid.");

        int skuIndex = Array.FindIndex(headers, h => h.Equals("Sku", StringComparison.OrdinalIgnoreCase));
        if (skuIndex == -1)
            throw new InvalidDataException("Required 'Sku' column not found.");

        while (!parser.EndOfData)
        {
            string[]? fields = parser.ReadFields();
            if (fields == null || fields.Length <= skuIndex)
                continue;

            string sku = fields[skuIndex];
            var product = new Product(sku);
            products.Add(product);
        }

        return products;
    }

    public List<Product> GetProductsFromXls(IFormFile file)
    {
        var products = new List<Product>();

        using var stream = file.OpenReadStream();
        var workbook = new HSSFWorkbook(stream);
        var sheet = workbook.GetSheetAt(0) ?? throw new InvalidDataException("Excel file has no worksheet.");
        var headerRow = sheet.GetRow(0) ?? throw new InvalidDataException("Excel file has no header row.");

        Dictionary<string, string[]> headerAliases = GetHeaderAliases();
        // Build a column index map from Excel headers
        var columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            var cellValue = headerRow.GetCell(i)?.ToString()?.Trim();
            if (string.IsNullOrEmpty(cellValue)) continue;

            foreach (var entry in headerAliases)
            {
                if (entry.Value.Contains(cellValue, StringComparer.OrdinalIgnoreCase))
                {
                    columnMap[entry.Key] = i;
                    break;
                }
            }
        }

        if (!columnMap.ContainsKey("Sku"))
        {
            throw new InvalidDataException("Required 'Sku' column not found.");
        }

        // Parse each row
        for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
        {
            var row = sheet.GetRow(rowIndex);
            if (row == null) continue;

            var sku = GetCellString(row, columnMap, "Sku");
            if (string.IsNullOrWhiteSpace(sku)) continue;

            var product = new Product(sku)
            {
                SupplierSku = GetCellString(row, columnMap, "SupplierSku"),
                Title = GetCellString(row, columnMap, "Title"),
                Description = GetCellString(row, columnMap, "Description"),
                EAN = GetCellString(row, columnMap, "EAN"),
                CategoryId = GetCellString(row, columnMap, "CategoryId"),
                Series = GetCellString(row, columnMap, "Series"),
                Color = GetCellString(row, columnMap, "Color"),
                Material = GetCellString(row, columnMap, "Material"),
                Price = GetCellInt(row, columnMap, "Price"),
                Cost = GetCellFloat(row, columnMap, "Cost"),
                Qty = GetCellInt(row, columnMap, "Qty"),
                Weight = GetCellFloat(row, columnMap, "Weight"),
                MainImage = GetCellString(row, columnMap, "MainImage")
            };
            products.Add(product);
        }

        return products;
    }

    private static Dictionary<string, string[]> GetHeaderAliases()
    {
        // Define aliases for headers, if a column name is spelled differently
        Dictionary<string, string[]> headerAliases = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "sku", new[] { "sku" } },
            { "suppliersku", new[] { "supplier sku", "supplier_sku", "p/n" } },
            { "title", new[] { "title", "name" } },
            { "description", new[] { "description", "product description" } },
            { "ean", new[] { "ean" } },
            { "categoryId", new[] { "categoryId", "category id", "category_id" } },
            { "series", new[] { "series" } },
            { "color", new[] { "color", "colour" } },
            { "material", new[] { "material" } },
            { "price", new[] { "price" } },
            { "cost", new[] { "cost" } },
            { "qty", new[] { "qty", "quantity" } },
            { "weight", new[] { "weight" } },
            { "mainImage", new[] { "mainImage", "main_image" } },
        };
        return headerAliases;
    }

    // Helpers
    private static string? GetCellString(IRow row, Dictionary<string, int> map, string column)
    {
        return map.TryGetValue(column, out var idx) ? row.GetCell(idx)?.ToString()?.Trim() : null;
    }

    private static int? GetCellInt(IRow row, Dictionary<string, int> map, string column)
    {
        if (!map.TryGetValue(column, out var idx)) return null;
        var cell = row.GetCell(idx);
        return int.TryParse(cell?.ToString(), out var value) ? value : null;
    }

    private static float? GetCellFloat(IRow row, Dictionary<string, int> map, string column)
    {
        if (!map.TryGetValue(column, out var idx)) return null;
        var cell = row.GetCell(idx);
        return float.TryParse(cell?.ToString(), out var value) ? value : null;
    }

}