using Application.Repositories.Weeklists;
using Domain.Common;
using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Weeklists;

public class WeeklistTaskLinkRepository : IWeeklistTaskLinkRepository
{
    private readonly AppDbContext _dbContext;

    public WeeklistTaskLinkRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddWeeklistTaskLinksAsync(List<WeeklistTaskLink> taskLinks)
    {
        await _dbContext.WeeklistTaskLinks.AddRangeAsync(taskLinks);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<OperationResult> UpdateTaskStatus(int weeklistId, TaskNameEnum currentTask, TaskStatusEnum newTaskStatus)
    {
        // Find the row in DB, where weeklistId and CurrentTask(Enum.id) has a match
        int taskId = (int)currentTask;
        var taskLink = await _dbContext.WeeklistTaskLinks.FirstOrDefaultAsync(link => link.WeeklistId == weeklistId && link.WeeklistTaskId == taskId);
        if (taskLink == null)
        {
            throw new Exception($"Task link for WeeklistId {weeklistId} and Task '{currentTask}' not found.");
        }
        // Update
        taskLink.WeeklistTaskStatusId = (int)newTaskStatus;
        await _dbContext.SaveChangesAsync();
        return OperationResult.Ok();
    }

    public async Task<OperationResult> AdvanceNextTask(int weeklistId, TaskNameEnum newTask)
    {
        int newTaskId = (int)newTask;

        // int nextTaskId = (int)currentTask + 1;

        var nextTaskLink = await _dbContext.WeeklistTaskLinks.FirstOrDefaultAsync(link => link.WeeklistId == weeklistId && link.WeeklistTaskId == newTaskId);

        if (nextTaskLink == null)
        {
            return OperationResult.Fail($"Next task (id {newTaskId}) not found for Weeklist {weeklistId}.");
        }
 
        if (nextTaskLink.WeeklistTaskStatusId == (int)TaskStatusEnum.Awaiting)
        {
            nextTaskLink.WeeklistTaskStatusId = (int)TaskStatusEnum.Ready;
            await _dbContext.SaveChangesAsync();
            return OperationResult.Ok();
        }

        return OperationResult.Ok();
    }

    public async Task<OperationResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, TaskNameEnum currentTask, TaskNameEnum newTask)
    {
        var updateResult = await UpdateTaskStatus(weeklistId, currentTask, TaskStatusEnum.Done);

        if (!updateResult.Success)
        {
            return updateResult;
        }

        var advanceResult = await AdvanceNextTask(weeklistId, newTask);

        if (!advanceResult.Success)
        {
            return advanceResult;
        }        
        return OperationResult.Ok();
    }
}
