using Domain.Entities.Products;


namespace Application.Files.Interfaces;

public interface ICsvService
{
  byte[] GenerateProductsReadyForAICSV(List<Product> products);
}