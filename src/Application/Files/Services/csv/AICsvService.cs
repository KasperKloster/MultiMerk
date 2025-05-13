using System.Text;
using Application.Files.Interfaces;
using Application.Files.Interfaces.csv;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Services.csv;

public class AICsvService : CsvBaseService, IAICsvService
{
    protected readonly IFileParser _fileParser;
    public AICsvService(IFileParser fileParser)
    {
        _fileParser = fileParser;
    }

    public FilesResult GetProductsFromAI(IFormFile file)
    {
        if (Path.GetExtension(file.FileName) != ".csv")
        {
            return FilesResult.Fail(message: "Invalid file extension.");
        }
        // Getting products from .xls
        List<Product> products = _fileParser.GetProductsFromAI(file);
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


}


