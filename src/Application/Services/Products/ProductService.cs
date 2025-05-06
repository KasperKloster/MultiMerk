using Application.Files.Interfaces;
using Application.Repositories;
using Application.Services.Interfaces.Products;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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

    public async Task<FilesResult> UpdateProductsFromFile(IFormFile file)
    {
        // Getting products from file
        List<Product> products;
        try
        {
            if (!IsSupportedXlsFile(file))
            {
                return FilesResult.Fail("Unsupported file type.");
            }
            products = _fileParser.GetProductsFromXls(file);
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Error getting products from file: {ex.Message}");
        }
        // Update products
        try
        {
            await _productRepository.UpdateRangeAsync(products);
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Could not update products: {ex.Message}");
        }
        return FilesResult.SuccessResult();
    }

    public FilesResult GetProductsFromOutOfStock(IFormFile file)
    {

        try
        {
            if (!IsSupportedXlsFile(file))
            {
                return FilesResult.Fail("Unsupported file type.");
            }

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
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Error updating with stock products: {ex.Message}");
        }
        return FilesResult.SuccessResult();
    }

    private static bool IsSupportedXlsFile(IFormFile file)
    {
        return string.Equals(Path.GetExtension(file.FileName), ".xls", StringComparison.OrdinalIgnoreCase);
    }


}
