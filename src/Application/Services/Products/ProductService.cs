using Application.Files.Interfaces;
using Application.Repositories;
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

    public async Task<FilesResult> UpdateProductsFromFile(IFormFile file)
    {
        List<Product> Products = [];
        // Getting products from file
        try
        {
            string FileExtension = GetFileExtension(file);
            if (FileExtension != "xls")
            {
                return FilesResult.Fail("Unsupported file type.");
            }
            
            if(FileExtension == "xls")
            {                
                Products = _fileParser.GetProductsFromXls(file: file);
            }
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Error getting products from file: {ex.Message}");
        }
        
        // Update products
        try
        {
            await _productRepository.UpdateRangeAsync(products: Products);            
        }
        catch (Exception ex)
        {
            
            return FilesResult.Fail($"Could not update products: {ex.Message}");
        }        
        
        // Return success        
        return FilesResult.SuccessResult();        
    }

    private static string GetFileExtension(IFormFile file)
    {
        string FileExtension = Path.GetExtension(file.FileName);
        // if(FileExtension == ".csv") {
        //     return "csv";
        // }
        if(FileExtension == ".xls") {
            return "xls";
        }
        
        return string.Empty;
    }
}
