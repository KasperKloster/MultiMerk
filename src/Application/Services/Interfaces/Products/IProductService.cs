using Domain.Entities.Files;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces.Products;

public interface IProductService
{
    Task<FilesResult> UpdateProductsFromFile(IFormFile file);

}
