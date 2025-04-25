using Domain.Entities.Products;

namespace Application.Repositories;

public interface IProductRepository
{
    Task AddRangeAsync(IEnumerable<Product> products);
}
