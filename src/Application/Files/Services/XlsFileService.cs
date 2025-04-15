using Application.Files.Interfaces;
using Application.Repositories;
using Domain.Models.Files;
using Domain.Models.Products;
using Domain.Models.Weeklists;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Services;

public class XlsFileService : IXlsFileService
{
    private readonly IFileParser _fileparser;
    private readonly IProductRepository _productRepository;
    private readonly IWeeklistRepository _weeklistRepository;

    public XlsFileService(IFileParser fileparser, IProductRepository productRepository, IWeeklistRepository weeklistRepository)
    {
        _fileparser = fileparser;
        _productRepository = productRepository;
        _weeklistRepository = weeklistRepository;
    }

    public async Task<FilesResult> CreateWeeklist(IFormFile file, Weeklist weeklist)
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

        // Insert weeklist first into DB, so we can get the ID
        try{            
            await _weeklistRepository.AddAsync(weeklist);
        } 
        catch (Exception ex) 
        {            
            return FilesResult.Fail($"An error occured while saving weeklist to database: {ex.Message}");
        }

        // Associate products with the saved weeklist
        foreach (var product in products)
        {
            product.WeeklistId = weeklist.Id; // Assign foreign key
        }

        // Insert products into DB
        try{
            await _productRepository.AddRangeAsync(products);
        } 
        catch (Exception ex) 
        {
            return FilesResult.Fail($"An error occured while saving products to database: {ex.Message}");
        }

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