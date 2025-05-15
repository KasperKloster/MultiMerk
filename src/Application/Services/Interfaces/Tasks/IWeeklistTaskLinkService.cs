using Domain.Common;
using Domain.Enums;

namespace Application.Services.Interfaces.Tasks;

public interface IWeeklistTaskLinkService
{
    Task<OperationResult> UpdateTaskStatus(int weeklistId, TaskNameEnum currentTask, TaskStatusEnum newTaskStatus);
    Task<OperationResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, TaskNameEnum currentTask, TaskNameEnum newTask);
}
