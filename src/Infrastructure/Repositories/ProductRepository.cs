using Application.Repositories;
using Domain.Models.Products;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddRangeAsync(IEnumerable<Product> products)
    {
        _dbContext.Products.AddRange(products);
        await _dbContext.SaveChangesAsync();
    }
}
