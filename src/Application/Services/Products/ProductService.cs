using Application.Repositories;
using Application.Services.Interfaces.Files;
using Application.Services.Interfaces.Products;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Products;

public class ProductService : IProductService
{
    private readonly IFileParser _fileParser;
    private readonly IProductRepository _productRepository;    

    public ProductService(IFileParser fileParser, IProductRepository productRepository)
    {
        _fileParser = fileParser;
        _productRepository = productRepository;        
    }
    public async Task<List<Product>> GetProductsFromWeeklist(int weeklistId)
    {
        try{
            List<Product> products = await _productRepository.GetProductsFromWeeklist(weeklistId);
            return products;
        } catch(Exception ex) {
            throw new Exception(ex.Message);
        }
    }

    public async Task<FilesResult> UpdateProductsFromFile(IFormFile file)
    {      
        try
        {
            // Getting products from file
            List<Product> products = _fileParser.GetProductsFromXls(file);
            // Update products
            await _productRepository.UpdateRangeAsync(products);
            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Error getting products from file: {ex.Message}");
        }
    }

    public FilesResult GetProductsFromOutOfStock(IFormFile file)
    {
        try
        {
            Dictionary<string, int> stockProducts = _fileParser.GetProductsFromOutOfStock(file);
            return FilesResult.SuccessResultWithOutOfStock(stockProducts);
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Error getting products from file: {ex.Message}");
        }
    }

    public async Task<FilesResult> UpdateProductsFromStockProducts(Dictionary<string, int> stockProducts)
    {
        try
        {
            await _productRepository.UpdateQtyFromStockProducts(stockProducts);
            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Error updating with stock products: {ex.Message}");
        }
    }


}
