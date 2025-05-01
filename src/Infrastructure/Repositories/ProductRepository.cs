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
        // All SKUS from updated products
        var skus = products.Select(p => p.Sku).ToList();
        // Find Existing products from SKU
        var existingProducts = await _dbContext.Products.Where(p => skus.Contains(p.Sku)).ToListAsync();

        foreach (Product updatedProduct in products)
        {
            var existingProduct = existingProducts.FirstOrDefault(p => p.Sku == updatedProduct.Sku);

            if (existingProduct != null)
            {
                // Only update if a new value is provided
                if (updatedProduct.SupplierSku != null)
                {
                    existingProduct.SupplierSku = updatedProduct.SupplierSku;
                }

                if (updatedProduct.Title != null)
                {
                    existingProduct.Title = updatedProduct.Title;
                }     

                if (updatedProduct.Description != null)
                {
                    existingProduct.Description = updatedProduct.Description;
                }          

                if (updatedProduct.EAN != null)
                {
                    existingProduct.EAN = updatedProduct.EAN;
                } 

                if (updatedProduct.CategoryId != null)
                {
                    existingProduct.CategoryId = updatedProduct.CategoryId;
                }

                if (updatedProduct.Series != null)
                {
                    existingProduct.Series = updatedProduct.Series;
                }                 

                if (updatedProduct.Color != null)
                {
                    existingProduct.Color = updatedProduct.Color;
                }
                    
                if (updatedProduct.Material != null)
                {
                    existingProduct.Material = updatedProduct.Material;
                }

                if (updatedProduct.Price != null)
                {
                    existingProduct.Price = updatedProduct.Price;
                }

                if (updatedProduct.Cost != null)
                {
                    existingProduct.Cost = updatedProduct.Cost;
                }  
                if (updatedProduct.Qty != null)
                {
                    existingProduct.Qty = updatedProduct.Qty;
                }  

                if (updatedProduct.Weight != null)
                {
                    existingProduct.Weight = updatedProduct.Weight;
                }    

                if (updatedProduct.MainImage != null)
                {
                    existingProduct.MainImage = updatedProduct.MainImage;
                }                                                       

                if (updatedProduct.WeeklistId.HasValue) 
                {
                    existingProduct.WeeklistId = updatedProduct.WeeklistId;
                }
            }            
        }

        await _dbContext.SaveChangesAsync();
    }
}
