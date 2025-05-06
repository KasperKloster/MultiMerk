using Domain.Common;
using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Domain.Enums;

namespace Application.Repositories.Weeklists;

public interface IWeeklistTaskLinkRepository
{
    Task AddWeeklistTaskLinksAsync(List<WeeklistTaskLink> taskLinks);
    Task<OperationResult> UpdateTaskStatus(int weeklistId, WeeklistTaskName currentTask, WeeklistTaskStatus newTaskStatus);
    Task<OperationResult> AdvanceNextTask(int weeklistId, WeeklistTaskName currentTask);
    Task<OperationResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, WeeklistTaskName currentTask, WeeklistTaskName newTask);
}
