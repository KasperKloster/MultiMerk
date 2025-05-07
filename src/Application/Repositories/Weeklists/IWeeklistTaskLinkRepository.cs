using Domain.Common;
using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Domain.Enums;

namespace Application.Repositories.Weeklists;

public interface IWeeklistTaskLinkRepository
{
    Task AddWeeklistTaskLinksAsync(List<WeeklistTaskLink> taskLinks);
    Task<OperationResult> UpdateTaskStatus(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskStatusEnum newTaskStatus);
    Task<OperationResult> AdvanceNextTask(int weeklistId, WeeklistTaskNameEnum currentTask);
    Task<OperationResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum newTask);
}
