using Application.Files.Interfaces;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Services;

public class XlsFileService : IXlsFileService
{
    private readonly IFileParser _fileparser;

    public XlsFileService(IFileParser fileparser)
    {
        _fileparser = fileparser;
    }

    public async Task<FilesResult> GetProductsFromXls(IFormFile file)
    {        
        // Is an .xls file        
        if(!HasValidXlsFileExtension(file)) 
        {
            return FilesResult.Fail(message: "Invalid file extension.");
        }
        // Getting products from .xls
        List<Product> products = _fileparser.GetProductsFromXls(file);
        if (products.Count == 0) 
        {
            return FilesResult.Fail("No products found in the file.");
        }        

        return FilesResult.SuccessResultWithProducts(products);
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