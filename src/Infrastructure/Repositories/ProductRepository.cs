using Application.Repositories;
using Domain.Entities.Products;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task UpdateRangeAsync(List<Product> products)
    {
        var skus = products.Select(p => p.Sku).ToList();

        var existingProducts = await _dbContext.Products.Where(p => skus.Contains(p.Sku)).ToListAsync();

        foreach (var updatedProduct in products)
        {
            var existingProduct = existingProducts
                .FirstOrDefault(p => p.Sku == updatedProduct.Sku);

            if (existingProduct != null)
            {
                // Only update if a new value is provided
                // if (updatedProduct.Color != null)
                //     existingProduct.Color = updatedProduct.Color;

                // if (updatedProduct.Material != null)
                //     existingProduct.Material = updatedProduct.Material;

                // if (updatedProduct.EAN != null)
                //     existingProduct.EAN = updatedProduct.EAN;

                if (updatedProduct.WeeklistId.HasValue) {
                    existingProduct.WeeklistId = updatedProduct.WeeklistId;
                }
            }            
        }

        await _dbContext.SaveChangesAsync();
    }
}
