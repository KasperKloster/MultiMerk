using Domain.Entities.Files;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces.Files;

public interface IXlsFileService
{    
    FilesResult GetProductsFromXls(IFormFile file);
    public byte[] GetProductChecklist(List<Product> products);
    public byte[] GetProductWarehouselist(List<Product> products);
}
