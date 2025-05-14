using Application.DTOs.Weeklists;
using Domain.Entities.Products;

namespace Application.Services.Interfaces.Files;

public interface IZipService
{
    Task<byte[]> CreateZipAdminImportAsync(WeeklistDto weeklist, List<Product> products);

}
