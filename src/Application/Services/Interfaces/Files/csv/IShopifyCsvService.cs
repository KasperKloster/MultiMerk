using Domain.Entities.Products;

namespace Application.Services.Interfaces.Files.csv;

public interface IShopifyCsvService
{
    byte[] GenerateShopifyDefaultImportCsv(List<Product> products);
}
