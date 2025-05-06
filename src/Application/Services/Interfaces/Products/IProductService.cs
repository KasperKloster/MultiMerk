using Domain.Entities.Files;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces.Products;

public interface IProductService
{
    Task<List<Product>> GetProductsFromWeeklist(int weeklistId);
    Task<FilesResult> UpdateProductsFromFile(IFormFile file);
    FilesResult GetProductsFromOutOfStock(IFormFile file);
    Task<FilesResult> UpdateProductsFromStockProducts(Dictionary<string, int> stockProducts);
}
