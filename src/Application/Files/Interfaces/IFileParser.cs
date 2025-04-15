using Domain.Models.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Interfaces;

public interface IFileParser
{
    char GetDelimiterFromCsv(IFormFile file);
    public List<Product> GetProductsFromCsv(IFormFile file, char delimiter);
    public List<Product> GetProductsFromXls(IFormFile file);
}
