using Domain.Entities.Products;

namespace Application.Services.Interfaces.Files.csv;

public interface IMagentoCsvService
{
    byte[] GenerateMagentoAdminImportCsv(List<Product> products);
    byte[] GenerateMagentoAttributeImportCsv(List<Product> products);
    byte[] GenerateMagentoDKKPricesImportCsv(List<Product> products);
    byte[] GenerateMagentoNOKPricesImportCsv(List<Product> products);
}
