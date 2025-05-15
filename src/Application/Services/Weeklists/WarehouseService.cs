using Application.DTOs.Weeklists;
using Application.Services.Interfaces.Files;
using Application.Services.Interfaces.Products;
using Application.Services.Interfaces.Tasks;
using Application.Services.Interfaces.Weeklists;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Weeklists;

public class WarehouseService : ServiceBase, IWarehouseService
{
    private readonly IProductService _productService;
    private readonly IXlsFileService _xlsFileService;
    private readonly IWeeklistService _weeklistService;
    public WarehouseService(IWeeklistTaskLinkService weeklistTaskLinkService, IProductService productService, IXlsFileService xlsFileService, IWeeklistService weeklistService) : base(weeklistTaskLinkService)
    {
        _productService = productService;
        _xlsFileService = xlsFileService;
        _weeklistService = weeklistService;
    }

    public async Task<FilesResult> GetChecklist(int weeklistId)
    {
        List<Product> products = await _productService.GetProductsFromWeeklist(weeklistId);
        byte[] xlsBytes = _xlsFileService.GetProductChecklist(products);
        // Get weeklist to create filename
        WeeklistDto weeklist = await _weeklistService.GetWeeklistAsync(weeklistId);
        string fileName = $"{weeklist.Number}-{weeklist.OrderNumber}({weeklist.ShippingNumber})-Checklist.xls";
        return FilesResult.SuccessWithFileExport(xlsBytes, fileName);
    }

    public async Task<FilesResult> UploadChecklistAndTaskAdvance(IFormFile file, int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum nextTask)
    {
        FilesResult result = await _productService.UpdateProductsFromXlsTaskAdvance(file, weeklistId, currentTask, nextTask);
        if (!result.Success)
        {
            return result; // return the failure result
        }
        return FilesResult.SuccessResult();
    }

    public async Task<FilesResult> GetWarehouselist(int weeklistId)
    {
        List<Product> products = await _productService.GetProductsFromWeeklist(weeklistId);
        byte[] xlsBytes = _xlsFileService.GetProductWarehouselist(products);
        WeeklistDto weeklist = await _weeklistService.GetWeeklistAsync(weeklistId);
        string fileName = $"{weeklist.Number}-Warehouselist.xls";
        return FilesResult.SuccessWithFileExport(xlsBytes, fileName);
    }
    public async Task<FilesResult> MarkCompleteAdvanceNext(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum nextTask)
    {
        // Update task
        FilesResult taskUpdateResult = await UpdateTaskStatusAndAdvanceNext(weeklistId: weeklistId, currentTask: currentTask, newTask: nextTask);
        if (!taskUpdateResult.Success)
        {
            return taskUpdateResult; // return the failure result
        }
        return FilesResult.SuccessResult();
    }
}
