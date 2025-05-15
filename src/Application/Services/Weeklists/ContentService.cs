using Application.DTOs.Weeklists;
using Application.Repositories;
using Application.Services.Interfaces.Files.csv;
using Application.Services.Interfaces.Tasks;
using Application.Services.Interfaces.Weeklists;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Weeklists;

public class ContentService : ServiceBase, IContentService
{
    private readonly IProductRepository _productRepository;
    private readonly IAICsvService _aiCsvService;
    private readonly IWeeklistService _weeklistService;

    public ContentService(IProductRepository productRepository, IAICsvService aiCsvService, IWeeklistService weeklistService, IWeeklistTaskLinkService weeklistTaskLinkService) : base(weeklistTaskLinkService)
    {
        _productRepository = productRepository;
        _aiCsvService = aiCsvService;
        _weeklistService = weeklistService;
    }

    public async Task<FilesResult> GetAIProductsAndTaskAdvance(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum nextTask)
    {
        List<Product> products = await GetProductsReadyForAI(weeklistId);
        var csvBytes = _aiCsvService.GenerateProductsReadyForAICSV(products);
        WeeklistDto weeklist = await _weeklistService.GetWeeklistAsync(weeklistId);
        var fileName = $"{weeklist.Number}-Ready-For-AI.csv";
        // Mark Current task as done, set next to ready                
        await UpdateTaskStatusAndAdvanceNext(weeklistId, WeeklistTaskNameEnum.GetAIContentList, WeeklistTaskNameEnum.UploadAIContent);
        return FilesResult.SuccessWithFileExport(csvBytes, fileName);
    }

    public async Task<FilesResult> InsertAIProductsUpdateStatus(IFormFile file, int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskStatusEnum taskStatus)
    {
        try
        {
            FilesResult aiProducts = _aiCsvService.GetProductsFromAI(file);
            if (!aiProducts.Success)
            {
                return FilesResult.Fail($"{aiProducts.Message}");
            }

            await UpdateProductsToAIContent(aiProducts.Products);

            // Mark Current task as done
            var updateTaskResult = await UpdateTaskStatus(weeklistId, WeeklistTaskNameEnum.UploadAIContent, WeeklistTaskStatusEnum.Done);
            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"{ex.Message}");
        }
    }

    private async Task<FilesResult> UpdateProductsToAIContent(List<Product> aiProducts)
    {
        try
        {
            await _productRepository.UpdateProductsFromAI(aiProducts);
            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<List<Product>> GetProductsReadyForAI(int weeklistId)
    {
        try
        {
            List<Product> products = await _productRepository.GetProductsReadyForAI(weeklistId);
            return products;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


}

