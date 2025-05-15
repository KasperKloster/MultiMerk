using Domain.Entities.Files;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces.Weeklists;

public interface IContentService
{
    Task<FilesResult> GetAIProductsAndTaskAdvance(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum nextTask);
    Task<FilesResult> InsertAIProductsUpdateStatus(IFormFile file, int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskStatusEnum taskStatus);    
}
