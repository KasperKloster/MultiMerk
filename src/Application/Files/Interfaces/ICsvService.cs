using Domain.Entities.Files;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;


namespace Application.Files.Interfaces;

public interface ICsvService
{
  FilesResult GetProductsFromAI(IFormFile file);
  byte[] GenerateProductsReadyForAICSV(List<Product> products);
  byte[] GenerateMagentoAdminImportCsv(List<Product> products);  
  byte[] GenerateMagentoAttributeImportCsv(List<Product> products);
  byte[] GenerateMagentoDKKPricesImportCsv(List<Product> products);
  byte[] GenerateMagentoNOKPricesImportCsv(List<Product> products);
}