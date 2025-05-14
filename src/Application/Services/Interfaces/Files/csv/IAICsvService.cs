using Domain.Entities.Files;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces.Files.csv;

public interface IAICsvService
{
    FilesResult GetProductsFromAI(IFormFile file);
    byte[] GenerateProductsReadyForAICSV(List<Product> products);
}
