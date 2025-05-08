using System.IO.Compression;
using Application.DTOs.Weeklists;
using Application.Files.Interfaces;
using Domain.Entities.Products;

namespace Application.Files.Services;

public class ZipService : IZipService
{
    private readonly ICsvService _csvService;

    public ZipService(ICsvService csvService)
    {
        _csvService = csvService;
    }

    public async Task<byte[]> CreateZipMagentoAdminImportAsync(WeeklistDto weeklist, List<Product> products)
    {
        // Generate files
        var files = new Dictionary<string, byte[]>
        {
            [$"1-OnlyAdd_Upload-{weeklist.Number}-admin.csv"] = _csvService.GenerateMagentoAdminImportCsv(products),
            [$"upload-{weeklist.Number}-admin-cat_loc.csv"] = _csvService.GenerateMagentoAttributeImportCsv(products),
            [$"upload-{weeklist.Number}-DK-prices.csv"] = _csvService.GenerateMagentoDKKPricesImportCsv(products),
            [$"upload-{weeklist.Number}-NO-prices.csv"] = _csvService.GenerateMagentoNOKPricesImportCsv(products),
            [$"Shopify-upload-{weeklist.Number}-Products.csv"] = _csvService.GenerateShopifyDefaultImportCsv(products),
        };

        using var memoryStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var file in files)
            {
                var entry = zipArchive.CreateEntry(file.Key);
                using var entryStream = entry.Open();
                await entryStream.WriteAsync(file.Value, 0, file.Value.Length);
            }
        }

        memoryStream.Position = 0;
        return memoryStream.ToArray(); // Return ZIP content as byte array
    }

    public Task<byte[]> CreateZipShopifyImportAsync(WeeklistDto weeklist, List<Product> products)
    {
        throw new NotImplementedException();
    }
}
