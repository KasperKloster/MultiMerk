using System;
using System.Diagnostics;
using Application.Repositories;
using Domain.Models.Products;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;
    
    public async Task AddRangeAsync(IEnumerable<Product> products)
    {
        try
        {
            _dbContext.Products.AddRange(products);
            await _dbContext.SaveChangesAsync();
        } catch(Exception e) {
            Debug.WriteLine($"Error adding products: {e.Message}");
        }
        
    }
}
