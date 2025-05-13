using Application.DTOs.Weeklists;
using Domain.Entities.Products;

namespace Application.Files.Interfaces;

public interface IZipService
{
    Task<byte[]> CreateZipAdminImportAsync(WeeklistDto weeklist, List<Product> products);    

}
