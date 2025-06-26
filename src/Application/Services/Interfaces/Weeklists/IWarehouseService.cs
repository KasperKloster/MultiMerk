using System;
using Domain.Entities.Files;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces.Weeklists;

public interface IWarehouseService
{
    Task<FilesResult> GetChecklist(int weeklistId);

    Task<FilesResult> UploadChecklistAndTaskAdvance(IFormFile file, int weeklistId, TaskNameEnum currentTask, TaskNameEnum nextTask);    

    Task<FilesResult> GetWarehouselist(int weeklistId);
    
    Task<FilesResult> MarkCompleteAdvanceNext(int weeklistId, TaskNameEnum currentTask, TaskNameEnum nextTask);
    
}
