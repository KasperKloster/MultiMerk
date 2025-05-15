using Application.Services.Interfaces.Tasks;
using Domain.Entities.Files;
using Domain.Enums;

namespace Application.Services;

public abstract class ServiceBase
{
    protected readonly IWeeklistTaskLinkService _weeklistTaskLinkService;

    protected ServiceBase(IWeeklistTaskLinkService weeklistTaskLinkService)
    {
        _weeklistTaskLinkService = weeklistTaskLinkService;
    }

    protected async Task<FilesResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum newTask)
    {
        try
        {
            var result = await _weeklistTaskLinkService.UpdateTaskStatusAndAdvanceNext(
                weeklistId: weeklistId,
                currentTask: currentTask,
                newTask: newTask);

            if (!result.Success)
            {
                return FilesResult.Fail("Could not update task and advance next");
            }
            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Could not update task and advance next: {ex.Message}");
        }
    }

    protected async Task<FilesResult> UpdateTaskStatus(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskStatusEnum newTaskStatus)
    {
        try
        {
            var result = await _weeklistTaskLinkService.UpdateTaskStatus(
                weeklistId: weeklistId,
                currentTask: currentTask,
                newTaskStatus: newTaskStatus);
            if (!result.Success) 
            {
                return FilesResult.Fail("Could not update task");
            }
            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"Could not update task: {ex.Message}");
        }
    }

}
