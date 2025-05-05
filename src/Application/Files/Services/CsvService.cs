using System.Text;
using Application.Files.Interfaces;
using Domain.Entities.Products;

namespace Application.Files.Services;

public class CsvService : ICsvService
{
    public byte[] GenerateProductsReadyForAICSV(List<Product> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Sku,Title,Description,Series"); // Header

        foreach (Product product in products)
        {
            sb.AppendLine($"{product.Sku},{Escape(product.Title)},{Escape(product.Description)},{product.Series}");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static string Escape(string input)
    {
        if (input.Contains(',') || input.Contains('"') || input.Contains('\n'))
        {
            return $"\"{input.Replace("\"", "\"\"")}\"";
        }            
        return input;
    }

}