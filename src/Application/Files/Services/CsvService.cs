using System.Globalization;
using System.Text;
using Application.Files.Interfaces;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Services;

public class CsvService : ICsvService
{
    private readonly IFileParser _fileparser;

    public CsvService(IFileParser fileparser)
    {
        _fileparser = fileparser;
    }

    public FilesResult GetProductsFromAI(IFormFile file)
    {
        if (Path.GetExtension(file.FileName) != ".csv")
        {
            return FilesResult.Fail(message: "Invalid file extension.");            
        }
        // Getting products from .xls
        List<Product> products = _fileparser.GetProductsFromAI(file);
        if (products.Count == 0)
        {
            return FilesResult.Fail("No products found in the file.");
        }

        return FilesResult.SuccessResultWithProducts(products);
    }

    public byte[] GenerateProductsReadyForAICSV(List<Product> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Sku;Title;Description;Series"); // Header
        foreach (Product product in products)
        {
            sb.AppendLine($"{product.Sku};{Escape(product.Title)};{Escape(product.Description)};{product.Series}");
        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    public byte[] GenerateMagentoAdminImportCsv(List<Product> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("supplier_sku;series;ean;sku;name;price;description;cost;weight;image;small_image;thumbnail;visibility;attribute_set_code;product_type;product_websites;product_online;news_from_date;options_container");
        foreach (Product product in products)
        {
            sb.AppendLine(
                $"{product.SupplierSku};" +
                $"{Escape(product.Series)};" +
                $"{Escape(product.EAN)};" +
                $"{Escape(product.Sku)};" +
                $"{Escape(product.Title)};" +
                $"{ConvertSEKToEUR(product.Price)};" +
                $"{Escape(product.Description)};" +
                $"{product.Cost};" +
                $"{product.Weight};" +
                $"{product.MainImage};" +
                $"{product.MainImage};" +
                $"{product.MainImage};" +
                $"\"Catalog, Search\";" +                
                $"\"Migration_Default\";" +
                $"\"simple\";" +
                $"\"base,se,dk,no,fi,nl,be,ie,de,ch\";" +
                $"\"1\";" +
                $"\"{DateTime.Today:MM/dd/yy}\";" +
                $"\"Block after Info Column\""
            );
        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    public byte[] GenerateMagentoAttributeImportCsv(List<Product> products)
    {        
        var sb = new StringBuilder();
        sb.AppendLine("sku;categories;location;product_type");
        foreach (Product product in products)
        {
            sb.AppendLine(                
                $"{Escape(product.Sku)};" +
                $"{Escape(product.CategoryId)};" +                
                $"{Escape(product.Location)};" +
                $"\"simple\""
            );
        }
        return Encoding.UTF8.GetBytes(sb.ToString());        
    }

    public byte[] GenerateMagentoDKKPricesImportCsv(List<Product> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("sku;price;product_type;store_view_code;product_websites");
        foreach (Product product in products)
        {
            sb.AppendLine(                
                $"{Escape(product.Sku)};" +
                $"{ConvertSEKToDKK(product.Price)};" +
                $"\"simple\";" +
                $"\"dk\";" +
                $"\"dk\""
            );
        }
        return Encoding.UTF8.GetBytes(sb.ToString());            
    }

    public byte[] GenerateMagentoNOKPricesImportCsv(List<Product> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("sku;price;product_type;store_view_code;product_websites");
        foreach (Product product in products)
        {
            sb.AppendLine(                
                $"{Escape(product.Sku)};" +
                $"{ConvertSEKToNOK(product.Price)};" +
                $"\"simple\";" +
                $"\"no\";" +
                $"\"no\""
            );
        }
        return Encoding.UTF8.GetBytes(sb.ToString());      
    }

    // Helpers
    private static string Escape(string? input)
    {
        if (string.IsNullOrEmpty(input)) return "";

        if (input.Contains(',') || input.Contains('"') || input.Contains('\n'))
        {
            return $"\"{input.Replace("\"", "\"\"")}\"";
        }

        return input;
    }

    private static string ConvertSEKToEUR(int? sek)
    {
        if (sek == null){
            return "0.00";
        }

        decimal currency = 0.11m;
        decimal value = sek.Value * currency;
        decimal result = Math.Floor(value / 0.05m) * 0.05m;
        return result.ToString("0.00", CultureInfo.InvariantCulture);
    }

    private static int ConvertSEKToNOK(int? sek)
    {
        int valueInSek = sek ?? 0; // Use 0 if null

        double value = valueInSek + valueInSek * 0.1;
        double roundedUpTo10 = Math.Ceiling(value / 10) * 10;
        return (int)(roundedUpTo10 - 1);
    }

    private static int ConvertSEKToDKK(int? sek)
    {
        int valueInSek = sek ?? 0;

        double discounted = valueInSek * 0.7;
        double roundedUpTo10 = Math.Ceiling(discounted / 10) * 10;
        return (int)(roundedUpTo10 - 1);
    }


}