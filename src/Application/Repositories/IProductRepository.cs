using Domain.Entities.Products;

namespace Application.Repositories;

public interface IProductRepository
{
    Task AddRangeAsync(IEnumerable<Product> products);
    Task UpdateRangeAsync(List<Product> products);
    Task<List<Product>> GetProductsFromWeeklist(int weeklistId);
    Task<List<Product>> GetProductsReadyForAI(int weeklistId);
    Task UpdateProductsFromAI(List<Product> aiProducts);
    Task UpdateQtyFromStockProducts(Dictionary<string, int> stockProducts);
}
