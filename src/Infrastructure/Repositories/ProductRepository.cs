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

                if (updatedProduct.TemplateId != null)
                {
                    existingProduct.TemplateId = updatedProduct.TemplateId;
                }

                if (updatedProduct.Location != null)
                {
                    existingProduct.Location = updatedProduct.Location;
                }

                if (updatedProduct.WeeklistId.HasValue)
                {
                    existingProduct.WeeklistId = updatedProduct.WeeklistId;
                }
            }
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Product>> GetProductsReadyForAI(int weeklistId)
    {
        return await _dbContext.Products
                    .Where(p => p.WeeklistId == weeklistId && p.TemplateId == null)
                    .GroupBy(p => p.Series ?? p.Sku) // Group by series, if there, else SKU
                    .Select(g => g.First())
                    .ToListAsync();
    }

    public async Task UpdateProductsFromAI(List<Product> aiProducts)
    {
        // Get all series values from AI products (ignoring nulls)
        var aiSeriesList = aiProducts
            .Where(p => !string.IsNullOrWhiteSpace(p.Series))
            .Select(p => p.Series)
            .Distinct()
            .ToList();

        // Get all SKUs from AI products that don't have a Series
        var aiSkuList = aiProducts
            .Where(p => string.IsNullOrWhiteSpace(p.Series))
            .Select(p => p.Sku)
            .Distinct()
            .ToList();

        // Query existing products that match by Series or by SKU (if no series)
        var existingProducts = await _dbContext.Products
            .Where(p => (p.Series != null && aiSeriesList.Contains(p.Series)) || aiSkuList.Contains(p.Sku))
            .ToListAsync();

        foreach (var aiProduct in aiProducts)
        {
            // Prefer match by Series if available, otherwise by SKU
            var existingProduct = !string.IsNullOrWhiteSpace(aiProduct.Series)
                ? existingProducts.FirstOrDefault(p => p.Series == aiProduct.Series)
                : existingProducts.FirstOrDefault(p => p.Sku == aiProduct.Sku);

            if (existingProduct != null)
            {
                // Update fields only if AI version has a value
                if (!string.IsNullOrWhiteSpace(aiProduct.Title)){
                    existingProduct.Title = aiProduct.Title;
                }

                if (!string.IsNullOrWhiteSpace(aiProduct.Description)){
                    existingProduct.Description = aiProduct.Description;
                }
            }
        }
        // Save changes to DB
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateQtyFromStockProducts(Dictionary<string, int> stockProducts)
    {
        // Extract the SupplierSkus from the dictionary
        var supplierSkus = stockProducts.Keys.ToList();

        // Find all matching products in the database
        var existingProducts = await _dbContext.Products.Where(p => p.SupplierSku != null && supplierSkus.Contains(p.SupplierSku)).ToListAsync();

        // Update the Qty field
        foreach (var product in existingProducts)
        {
            if (product.SupplierSku != null && stockProducts.TryGetValue(product.SupplierSku, out int stockQty))
            {
                // Ensure Qty is not null before subtraction
                product.Qty = (product.Qty ?? 0) - stockQty;
            }
        }
        // Save changes to database
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Product>> GetProductsFromWeeklist(int weeklistId)
    {
        return await _dbContext.Products.Where(p => p.WeeklistId == weeklistId).ToListAsync();
    }

}
