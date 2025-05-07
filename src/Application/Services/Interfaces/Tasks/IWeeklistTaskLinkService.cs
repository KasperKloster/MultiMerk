using System;
using Domain.Common;
using Domain.Enums;

namespace Application.Services.Interfaces.Tasks;

public interface IWeeklistTaskLinkService
{
    Task<OperationResult> UpdateTaskStatus(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskStatusEnum newTaskStatus);
    Task<OperationResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum newTask);
}
