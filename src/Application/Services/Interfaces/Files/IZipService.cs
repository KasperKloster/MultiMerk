using Domain.Enums;

namespace Application.Services.Interfaces.Files;

public interface IZipService
{
    Task<byte[]> GetZipAdminImportUpdateStatus(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskStatusEnum taskStatus);    
}
