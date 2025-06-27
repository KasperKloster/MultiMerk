using System.Threading.Tasks;
using Application.Services.Interfaces.Files;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Application.Services.Files;
public class XlsFileService : IXlsFileService
{
    private readonly IFileParser _fileparser;    
    public XlsFileService(IFileParser fileparser)
    {
        _fileparser = fileparser;        
    }
    public async Task<FilesResult> GetProductsFromXls(IFormFile file)
    {        
        // Getting products from .xls
        List<Product> products = await _fileparser.GetProductsFromXls(file);
        if (products is null || products.Count == 0)
        {
            return FilesResult.Fail("No products found in the file.");
        }
        return FilesResult.SuccessResultWithProducts(products);
    }
    public byte[] GetProductChecklist(List<Product> products)
    {
        IWorkbook workbook = new HSSFWorkbook();
        var sheet = workbook.CreateSheet("Products");

        // Header row
        IRow headerRow = sheet.CreateRow(0);
        headerRow.CreateCell(0).SetCellValue("Supplier SKU");
        headerRow.CreateCell(1).SetCellValue("Title");
        headerRow.CreateCell(2).SetCellValue("SKU");
        headerRow.CreateCell(3).SetCellValue("Description");
        headerRow.CreateCell(4).SetCellValue("Qty");                       

        // Data
        for (int i = 0; i < products.Count; i++)
        {
            var row = sheet.CreateRow(i + 1);
            var product = products[i];
            row.CreateCell(0).SetCellValue(product.SupplierSku ?? "");
            row.CreateCell(1).SetCellValue(product.Title ?? "");
            row.CreateCell(2).SetCellValue(product.Sku);
            row.CreateCell(3).SetCellValue(product.Description ?? "");
            if (product.Qty.HasValue){
                row.CreateCell(4).SetCellValue(product.Qty.Value);
            }
            else{
                row.CreateCell(4).SetCellType(CellType.Blank);
            }
        }

        using var stream = new MemoryStream();
        workbook.Write(stream);
        return stream.ToArray();
    }

    public byte[] GetProductWarehouselist(List<Product> products)
    {
        IWorkbook workbook = new HSSFWorkbook(); // HSSF = .xls
        ISheet sheet = workbook.CreateSheet("Products");

        // Header row
        IRow headerRow = sheet.CreateRow(0);
        headerRow.CreateCell(0).SetCellValue("SKU");
        headerRow.CreateCell(1).SetCellValue("Title");
        headerRow.CreateCell(2).SetCellValue("Qty");
        headerRow.CreateCell(3).SetCellValue("Location");

        // Data rows
        for (int i = 0; i < products.Count; i++)
        {
            var row = sheet.CreateRow(i + 1);
            var product = products[i];
            row.CreateCell(0).SetCellValue(product.Sku ?? "");
            row.CreateCell(1).SetCellValue(product.Title ?? "");
            if (product.Qty.HasValue){
                row.CreateCell(2).SetCellValue(product.Qty.Value);
            }
            else{
                row.CreateCell(2).SetCellType(CellType.Blank);
            }
            row.CreateCell(3).SetCellValue(product.Location ?? "");
        }

        using var stream = new MemoryStream();
        workbook.Write(stream);
        return stream.ToArray();
    }
}