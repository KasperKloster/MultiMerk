using Application.Repositories;
using Application.Services.Interfaces.Weeklists;
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
}

