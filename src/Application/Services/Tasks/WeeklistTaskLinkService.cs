using System.Threading.Tasks;
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
    public async Task<OperationResult> UpdateTaskStatus(int weeklistId, WeeklistTaskName currentTask, WeeklistTaskStatus taskStatus)
    {
        return await _weeklistTaskLinkRepository.UpdateTaskStatus(weeklistId, currentTask, taskStatus);
    }

    public async Task<OperationResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, WeeklistTaskName currentTask, WeeklistTaskStatus taskStatus)
    {
        return await _weeklistTaskLinkRepository.UpdateTaskStatusAndAdvanceNext(weeklistId, currentTask, taskStatus);
    }
}
