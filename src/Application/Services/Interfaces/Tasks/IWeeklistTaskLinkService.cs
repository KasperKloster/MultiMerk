using System;
using Domain.Common;
using Domain.Enums;

namespace Application.Services.Interfaces.Tasks;

public interface IWeeklistTaskLinkService
{
    Task<OperationResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, WeeklistTaskName currentTask, WeeklistTaskStatus taskStatus);
}
