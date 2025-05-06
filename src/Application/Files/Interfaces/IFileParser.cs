using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Interfaces;

public interface IFileParser
{        
    public List<Product> GetProductsFromXls(IFormFile file);
    public Dictionary<string, int> GetProductsFromOutOfStock(IFormFile file);    
}
