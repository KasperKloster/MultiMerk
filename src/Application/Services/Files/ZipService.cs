using System.IO.Compression;
using Application.DTOs.Weeklists;
using Application.Services.Interfaces.Files;
using Application.Services.Interfaces.Files.csv;
using Application.Services.Interfaces.Products;
using Application.Services.Interfaces.Tasks;
using Application.Services.Interfaces.Weeklists;
using Domain.Entities.Products;
using Domain.Enums;

namespace Application.Services.Files;

public class ZipService : ServiceBase, IZipService
{
    private readonly IWeeklistService _weeklistService;    
    private readonly IProductService _productService;
    private readonly IMagentoCsvService _magentoCsvService;
    private readonly IShopifyCsvService _shopifyCsvService;

    public ZipService(IWeeklistService weeklistService, IProductService productService, IMagentoCsvService magentoCsvService, IShopifyCsvService shopifyCsvService, IWeeklistTaskLinkService weeklistTaskLinkService) : base(weeklistTaskLinkService)
    {
        _weeklistService = weeklistService;
        _productService = productService;
        _magentoCsvService = magentoCsvService;
        _shopifyCsvService = shopifyCsvService;
    }

    public async Task<byte[]> GetZipAdminImportUpdateStatus(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskStatusEnum taskStatus)
    {
        WeeklistDto weeklist = await _weeklistService.GetWeeklistAsync(weeklistId);
        List<Product> products = await _productService.GetProductsFromWeeklist(weeklistId);
        byte[] zipBytes = await CreateZipAdminImportAsync(weeklist, products);
        await UpdateTaskStatus(weeklistId, currentTask, taskStatus);
        return zipBytes;
    }

    private async Task<byte[]> CreateZipAdminImportAsync(WeeklistDto weeklist, List<Product> products)
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
