using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces.Files;

public interface IFileParser
{        
    public List<Product> GetProductsFromXls(IFormFile file);
    public List<Product> GetProductsFromAI(IFormFile file);
    public Dictionary<string, int> GetProductsFromOutOfStock(IFormFile file);    
}
