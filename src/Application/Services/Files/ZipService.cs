using System.IO.Compression;
using Application.DTOs.Weeklists;
using Application.Services.Interfaces.Files;
using Application.Services.Interfaces.Files.csv;
using Domain.Entities.Products;

namespace Application.Services.Files;

public class ZipService : IZipService
{    
    private readonly IShopifyCsvService _shopifyCsvService;
    private readonly IMagentoCsvService _magentoCsvService;

    public ZipService(IShopifyCsvService shopifyCsvService, IMagentoCsvService magentoCsvService)
    {        
        _shopifyCsvService = shopifyCsvService;
        _magentoCsvService = magentoCsvService;
    }

    public async Task<byte[]> CreateZipAdminImportAsync(WeeklistDto weeklist, List<Product> products)
    {
        // Generate files
        var files = new Dictionary<string, byte[]>
        {
            [$"1-OnlyAdd_Upload-{weeklist.Number}-admin.csv"] = _magentoCsvService.GenerateMagentoAdminImportCsv(products),
            [$"upload-{weeklist.Number}-admin-cat_loc.csv"] = _magentoCsvService.GenerateMagentoAttributeImportCsv(products),
            [$"upload-{weeklist.Number}-DK-prices.csv"] = _magentoCsvService.GenerateMagentoDKKPricesImportCsv(products),
            [$"upload-{weeklist.Number}-NO-prices.csv"] = _magentoCsvService.GenerateMagentoNOKPricesImportCsv(products),
            [$"Shopify-upload-{weeklist.Number}-Products.csv"] = _shopifyCsvService.GenerateShopifyDefaultImportCsv(products),            
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
}
