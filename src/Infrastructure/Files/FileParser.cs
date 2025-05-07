using System.Globalization;
using Application.Files.Interfaces;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Infrastructure.Files;

public class FileParser : IFileParser
{

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
                MainImage = GetCellString(row, columnMap, "MainImage"),
                TemplateId = GetCellInt(row, columnMap, "Template"),
                Location = GetCellString(row, columnMap, "Location"),
            };
            products.Add(product);
        }

        return products;
    }

    public List<Product> GetProductsFromAI(IFormFile file)
    {
        // List to hold the final Product objects        
        var products = new List<Product>();
        
        // Temporary list to hold each CSV row as a dictionary (column name -> value)
        var result = new List<Dictionary<string, string>>();

        // Open and read the uploaded CSV file
        using var reader = new StreamReader(file.OpenReadStream());
        using var parser = new TextFieldParser(reader)
        {
            TextFieldType = FieldType.Delimited
        };
        
        // Delimiter
        parser.SetDelimiters(";");

        string[]? headers = null;
        
        // Remove spaces, lowercase, etc. from headers
        if (!parser.EndOfData)
        {
            headers = parser.ReadFields()?.Select(h => h.Trim().ToLowerInvariant().Replace(" ", "").Replace("_", "")).ToArray();
        }

        // Parse each row into a dictionary based on headers
        while (!parser.EndOfData)
        {
            var fields = parser.ReadFields();
            if (headers == null || fields == null || fields.Length != headers.Length){
                continue;
            }
            var row = headers.Zip(fields, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value?.Trim());
            result.Add(row);
        }
        
        // Convert each dictionary into a Product object
        foreach (var row in result)
        {
            // Sku is an required field
            if (!row.TryGetValue("sku", out var sku) || string.IsNullOrWhiteSpace(sku)){
                continue;
            }
            // Populate the Products
            var product = new Product(sku)
            {                
                Title = row.GetValueOrDefault("title"),
                Description = row.GetValueOrDefault("description"),
                Series = row.GetValueOrDefault("series"),            
            };
            products.Add(product);
        }
        return products;
    }

    public Dictionary<string, int> GetProductsFromOutOfStock(IFormFile file)
    {
        var outOfStockProducts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        using var stream = file.OpenReadStream();
        var workbook = new HSSFWorkbook(stream);
        var sheet = workbook.GetSheetAt(0) ?? throw new InvalidDataException("Excel file has no worksheet.");

        var headerAliases = GetHeaderAliases();
        IRow headerRow = null;
        int headerRowIndex = -1;

        // Step 1: Find header row dynamically
        for (int i = 0; i <= sheet.LastRowNum; i++)
        {
            var row = sheet.GetRow(i);
            if (row == null) continue;

            int matchCount = 0;
            for (int j = 0; j < row.LastCellNum; j++)
            {
                var cellValue = row.GetCell(j)?.ToString()?.Trim();
                if (string.IsNullOrEmpty(cellValue)) continue;

                foreach (var entry in headerAliases)
                {
                    if (entry.Value.Contains(cellValue, StringComparer.OrdinalIgnoreCase))
                    {
                        matchCount++;
                        break;
                    }
                }
            }

            if (matchCount >= 2)
            {
                headerRow = row;
                headerRowIndex = i;
                break;
            }
        }

        if (headerRow == null)
            throw new InvalidDataException("Could not detect header row in Excel file.");

        // Step 2: Build column map
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

        if (!columnMap.ContainsKey("SupplierSku") || !columnMap.ContainsKey("Qty"))
        {
            throw new InvalidDataException("Required columns 'SupplierSku' and 'Qty' not found.");
        }

        // Step 3: Parse rows
        for (int rowIndex = headerRowIndex + 1; rowIndex <= sheet.LastRowNum; rowIndex++)
        {
            var row = sheet.GetRow(rowIndex);
            if (row == null) continue;

            var supplierSku = GetCellString(row, columnMap, "SupplierSku");
            if (string.IsNullOrWhiteSpace(supplierSku)) continue;

            var qty = GetCellInt(row, columnMap, "Qty");

            // Optionally skip zero-qty items
            if (qty == 0) continue;

            // Handle duplicates gracefully
            outOfStockProducts[supplierSku] = qty ?? 0;
        }

        return outOfStockProducts;
    }

    // Helpers fpr .xls
    private static string? GetCellString(IRow row, Dictionary<string, int> map, string column)
    {
        return map.TryGetValue(column, out var idx) ? row.GetCell(idx)?.ToString()?.Trim() : null;
    }

    private static int? GetCellInt(IRow row, Dictionary<string, int> map, string column)
    {
        if (!map.TryGetValue(column, out var idx)) return null;
        var cell = row.GetCell(idx);
        if (cell == null) return null;

        var cellValue = cell.ToString()?.Trim();
        if (string.IsNullOrWhiteSpace(cellValue)) return null;

        return int.TryParse(cellValue, out var value) ? value : null;
    }

    private static float? GetCellFloat(IRow row, Dictionary<string, int> map, string column)
    {
        if (!map.TryGetValue(column, out var idx)) return null;
        var cell = row.GetCell(idx);
        return float.TryParse(cell?.ToString(), out var value) ? value : null;
    }

    // Helpers for .csv



    // Both helpers
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
            { "categoryId", new[] { "categoryId", "category id", "category_id", "cat id" } },
            { "series", new[] { "series" } },
            { "color", new[] { "color", "colour" } },
            { "material", new[] { "material" } },
            { "price", new[] { "price" } },
            { "cost", new[] { "cost" } },
            { "qty", new[] { "qty", "qty (unit)", "quantity" } },
            { "weight", new[] { "weight" } },
            { "mainImage", new[] { "mainImage", "main_image", "main image" } },
            { "template", new[] { "template" } },
            { "location", new[] { "location" } },
        };
        return headerAliases;
    }


}