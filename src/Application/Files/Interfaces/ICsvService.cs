using Domain.Entities.Products;


namespace Application.Files.Interfaces;

public interface ICsvService
{
  byte[] GenerateProductsReadyForAICSV(List<Product> products);
  byte[] GenerateMagentoAdminImportCsv(List<Product> products);  
  byte[] GenerateMagentoAttributeImportCsv(List<Product> products);
  byte[] GenerateMagentoDKKPricesImportCsv(List<Product> products);
  byte[] GenerateMagentoNOKPricesImportCsv(List<Product> products);
}