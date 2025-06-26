using Domain.Common;
using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Domain.Enums;

namespace Application.Repositories.Weeklists;

public interface IWeeklistTaskLinkRepository
{
    Task AddWeeklistTaskLinksAsync(List<WeeklistTaskLink> taskLinks);
    Task<OperationResult> UpdateTaskStatus(int weeklistId, TaskNameEnum currentTask, TaskStatusEnum newTaskStatus);
    Task<OperationResult> AdvanceNextTask(int weeklistId, TaskNameEnum currentTask);
    Task<OperationResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, TaskNameEnum currentTask, TaskNameEnum newTask);
}
