using System;
using Domain.Enums;

namespace Application.Services.Interfaces.Tasks;

public interface IWeeklistTaskLinkService
{
    void UpdateTaskStatus(int weeklistId, WeeklistTaskName currentTask, WeeklistTaskStatus taskStatus);
}
