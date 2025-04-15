using Application.Files.Interfaces;
using Domain.Constants;
using Domain.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using NPOI.HSSF.UserModel;

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
        var sheet = workbook.GetSheetAt(0);

        if (sheet == null)
        {
            throw new InvalidDataException("Excel file has no worksheet.");
        }

        // Assume first row is header
        var headerRow = sheet.GetRow(0);
        int skuColumnIndex = -1;

        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            var cell = headerRow.GetCell(i);
            if (cell != null && cell.StringCellValue.Equals("Sku", StringComparison.OrdinalIgnoreCase))
            {
                skuColumnIndex = i;
                break;
            }
        }

        if (skuColumnIndex == -1)
        {
            throw new InvalidDataException("Required 'Sku' column not found.");
        }

        // Loop through the rest of the rows
        for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
        {
            var row = sheet.GetRow(rowIndex);
            if (row == null) continue;

            var cell = row.GetCell(skuColumnIndex);
            var sku = cell?.ToString();
            if (!string.IsNullOrWhiteSpace(sku))
            {
                products.Add(new Product(sku));
            }
        }
        return products;
    }
}
