using Domain.Entities.Products;

namespace Application.Files.Interfaces.csv;

public interface IShopifyCsvService
{
    byte[] GenerateShopifyDefaultImportCsv(List<Product> products);
}
