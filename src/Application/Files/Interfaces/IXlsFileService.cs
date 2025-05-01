using Domain.Entities.Files;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Interfaces;

public interface IXlsFileService
{    
    Task<FilesResult> GetProductsFromXls(IFormFile file);
}
