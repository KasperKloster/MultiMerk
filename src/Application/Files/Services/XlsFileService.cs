using System;
using System.Threading.Tasks;
using Application.Files.Interfaces;
using Application.Repositories;
using Domain.Models.Files;
using Domain.Models.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Services;

public class XlsFileService : IXlsFileService
{
    private readonly IFileParser _fileparser;
    private readonly IProductRepository _productRepository;

    public XlsFileService(IFileParser fileparser, IProductRepository productRepository)
    {
        _fileparser = fileparser;
        _productRepository = productRepository;
    }

    public async Task<FilesResult> CreateWeeklist(IFormFile file)
    {
        // Is an .xls file
        bool HasXlsFileExtension = HasValidXlsFileExtension(file);
        if(!HasXlsFileExtension) {
            return FilesResult.Fail(message: "Invalid file extension.");
        }

        // Getting products from .xls
        List<Product> products = _fileparser.GetProductsFromXls(file);
        if (products.Count == 0) 
        {
            return FilesResult.Fail("No products found in the file.");
        }

        // Insert products into DB
        await _productRepository.AddRangeAsync(products);

        // Return success        
        return FilesResult.SuccessResult();
    }
    
    private static bool HasValidXlsFileExtension(IFormFile file)
    {
        string FileExtension = Path.GetExtension(file.FileName);
        if(FileExtension != ".xls") {
            return false;
        }
        return true;
    }
}
