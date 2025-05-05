using Domain.Entities.Files;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Interfaces;

public interface IXlsFileService
{    
    Task<FilesResult> GetProductsFromXls(IFormFile file);
    public byte[] GetProductChecklist(List<Product> products);
}
