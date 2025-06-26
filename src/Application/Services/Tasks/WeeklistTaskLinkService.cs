using Application.Repositories.Weeklists;
using Application.Services.Interfaces.Tasks;
using Domain.Common;
using Domain.Enums;

namespace Application.Services.Tasks;

public class WeeklistTaskLinkService : IWeeklistTaskLinkService
{
    private readonly IWeeklistTaskLinkRepository _weeklistTaskLinkRepository;

    public WeeklistTaskLinkService(IWeeklistTaskLinkRepository weeklistTaskLinkRepository)
    {
        _weeklistTaskLinkRepository = weeklistTaskLinkRepository;
    }
    public async Task<OperationResult> UpdateTaskStatus(int weeklistId, TaskNameEnum currentTask, TaskStatusEnum newTaskStatus)
    {
        return await _weeklistTaskLinkRepository.UpdateTaskStatus(weeklistId, currentTask, newTaskStatus);
    }

    public async Task<OperationResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, TaskNameEnum currentTask, TaskNameEnum newTask)
    {
        return await _weeklistTaskLinkRepository.UpdateTaskStatusAndAdvanceNext(weeklistId, currentTask, newTask);
    }
}
