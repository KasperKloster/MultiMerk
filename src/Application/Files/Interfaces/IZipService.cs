using Application.DTOs.Weeklists;
using Domain.Entities.Products;

namespace Application.Files.Interfaces;

public interface IZipService
{
    Task<byte[]> CreateZipMagentoAdminImportAsync(WeeklistDto weeklist, List<Product> products);
    Task<byte[]> CreateZipShopifyImportAsync(WeeklistDto weeklist, List<Product> products);

}
