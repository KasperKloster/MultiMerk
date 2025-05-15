using System;
using Domain.Entities.Files;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces.Weeklists;

public interface IWarehouseService
{
    Task<FilesResult> GetChecklist(int weeklistId);

    Task<FilesResult> UploadChecklistAndTaskAdvance(IFormFile file, int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum nextTask);    

    Task<FilesResult> GetWarehouselist(int weeklistId);
    
    Task<FilesResult> MarkCompleteAdvanceNext(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum nextTask);
    
}
