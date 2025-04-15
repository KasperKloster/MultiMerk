using Application.Files.Interfaces;
using Application.Repositories;
using Domain.Models.Files;
using Domain.Models.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Services;

public class CsvService : ICsvService
{
    private readonly IFileParser _fileparser;
    private readonly IProductRepository _productRepository;

    public CsvService(IFileParser fileparser)
    {
        _fileparser = fileparser;
    }

    public async Task<FilesResult> UploadCsv(IFormFile file)
    {
        // Checks if valid extension
        bool isFileExtensionValid = HasValidCsvFileExtension(file);
        if (!isFileExtensionValid)
        {
            return FilesResult.Fail(message: "Invalid file extension.");
        }

        // Getting delimiter
        char delimiter = GetDelimiterFromCsv(file);
        
        // Parse / Getting products from .csv
        List<Product> products = _fileparser.GetProductsFromCsv(file, delimiter);
        if (products.Count == 0) 
        {
            return FilesResult.Fail("No products found in the CSV file.");
        }
        
        // Insert into DB
        await _productRepository.AddRangeAsync(products);  
        
        // return success message
        return FilesResult.SuccessResult();
    }

    private static bool HasValidCsvFileExtension(IFormFile file)
    {
        string FileExtension = Path.GetExtension(file.FileName);
        if(FileExtension != ".csv") {
            return false;
        }
        return true;
    }

    private char GetDelimiterFromCsv(IFormFile file)
    {
        char delimiter = _fileparser.GetDelimiterFromCsv(file);
        return delimiter;
    }

}