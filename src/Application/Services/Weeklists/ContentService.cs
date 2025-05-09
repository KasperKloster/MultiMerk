using Application.Repositories;
using Application.Services.Interfaces.Weeklists;
using Domain.Entities.Files;
using Domain.Entities.Products;

namespace Application.Services.Weeklists;

public class ContentService : IContentService
{
    private readonly IProductRepository _productRepository;

    public ContentService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetProductsReadyForAI(int weeklistId)
    {
        try
        {
            List<Product> products = await _productRepository.GetProductsReadyForAI(weeklistId);
            return products;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<FilesResult> InsertAIProductContent(List<Product> aiProducts)
    {
        try
        {
            await _productRepository.UpdateProductsFromAI(aiProducts);            
            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

