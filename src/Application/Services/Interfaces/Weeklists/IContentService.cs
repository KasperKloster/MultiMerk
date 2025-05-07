using System;
using Domain.Entities.Files;
using Domain.Entities.Products;

namespace Application.Services.Interfaces.Weeklists;

public interface IContentService
{
    Task<List<Product>> GetProductsReadyForAI(int weeklistId);
    Task<FilesResult> InsertAIProductContent(List<Product> aiProducts);
}
