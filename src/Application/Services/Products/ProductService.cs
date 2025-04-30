using Application.Services.Interfaces.Products;
using Domain.Entities.Files;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Products;

public class ProductService : IProductService
{
    public Task<FilesResult> UpdateProductsFromFile(IFormFile file)
    {
        throw new NotImplementedException();
    }
}
