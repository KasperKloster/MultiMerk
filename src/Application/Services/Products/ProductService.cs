using Application.Repositories;
using Application.Services.Interfaces.Files;
using Application.Services.Interfaces.Products;
using Application.Services.Interfaces.Tasks;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Products;

public class ProductService : ServiceBase, IProductService
{
    private readonly IFileParser _fileParser;
    private readonly IProductRepository _productRepository;

    public ProductService(IFileParser fileParser, IProductRepository productRepository, IWeeklistTaskLinkService weeklistTaskLinkService) : base(weeklistTaskLinkService)
    {
        _fileParser = fileParser;
        _productRepository = productRepository;
    }
    public async Task<List<Product>> GetProductsFromWeeklist(int weeklistId)
    {
        try
        {
            List<Product> products = await _productRepository.GetProductsFromWeeklist(weeklistId);
            return products;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<FilesResult> UpdateProductsFromXlsUpdateStatus(IFormFile file, int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskStatusEnum taskStatus)
    {
        try
        {
            await UpdateProductsFromXlsFile(file);
            await UpdateTaskStatus(weeklistId, currentTask, taskStatus);
            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Error getting products from file: {ex.Message}");
        }
    }

    public async Task<FilesResult> UpdateProductsFromXlsTaskAdvance(IFormFile file, int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum nextTask)
    {
        try
        {
            await UpdateProductsFromXlsFile(file);
            await UpdateTaskStatusAndAdvanceNext(weeklistId, currentTask, nextTask);
            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Error getting products from file: {ex.Message}");
        }
    }

    public async Task<FilesResult> UpdateOutOfStockAndTaskAdvance(IFormFile file, int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum nextTask)
    {
        try
        {
            Dictionary<string, int> stockProducts = _fileParser.GetProductsFromOutOfStock(file);
            if (stockProducts == null || stockProducts.Count == 0)
            {
                return FilesResult.Fail("No stock products found in file.");
            }

            // Update products
            await _productRepository.UpdateQtyFromStockProducts(stockProducts);

            // Update task
            FilesResult taskUpdateResult = await UpdateTaskStatusAndAdvanceNext(weeklistId: weeklistId, currentTask: currentTask, newTask: nextTask);
            if (!taskUpdateResult.Success)
            {
                return taskUpdateResult; // return the failure result
            }

            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Error updating with stock products: {ex.Message}");
        }
    }

    private async Task<FilesResult> UpdateProductsFromXlsFile(IFormFile file)
    {
        try
        {
            // Getting products from file
            List<Product> products = _fileParser.GetProductsFromXls(file);
            // Update products
            await _productRepository.UpdateRangeAsync(products);
            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return FilesResult.Fail(ex.Message);
        }
    }


}
