using Application.Files.Interfaces;
using ClosedXML.Excel;
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
        if (!HasValidXlsFileExtension(file))
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
        if (FileExtension != ".xls")
        {
            return false;
        }
        return true;
    }

    public byte[] GetProductChecklist(List<Product> products)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Products");

        // Header
        worksheet.Cell(1, 1).Value = "Sku";
        worksheet.Cell(1, 2).Value = "Title";
        worksheet.Cell(1, 3).Value = "Qty";
        worksheet.Cell(1, 4).Value = "Location";

        // Data
        for (int i = 0; i < products.Count; i++)
        {
            var row = i + 2;
            var product = products[i];
            worksheet.Cell(row, 1).Value = product.Sku;
            worksheet.Cell(row, 2).Value = product.Title;
            worksheet.Cell(row, 3).Value = product.Qty;
            worksheet.Cell(row, 4).Value = product.Location;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}