using Domain.Entities.Files;
using Domain.Entities.Products;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces.Products;

public interface IProductService
{
    Task<List<Product>> GetProductsFromWeeklist(int weeklistId);
    Task<FilesResult> UpdateProductsFromXlsUpdateStatus(IFormFile file, int weeklistId, TaskNameEnum currentTask, TaskStatusEnum taskStatus);
    Task<FilesResult> UpdateProductsFromXlsTaskAdvance(IFormFile file, int weeklistId, TaskNameEnum currentTask, TaskNameEnum nextTask);
    Task<FilesResult> UpdateOutOfStockAndTaskAdvance(IFormFile file, int weeklistId, TaskNameEnum currentTask, TaskNameEnum nextTask);
       
}
