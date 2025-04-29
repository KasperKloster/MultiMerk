using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Domain.Entities.Weeklists.WeeklistTasks;

namespace Domain.Entities.Weeklists.Factories;

public static class WeeklistTaskLinkFactory
{
    public static List<WeeklistTaskLink> CreateLinks(int weeklistId, List<WeeklistTask> allTasks, int firstTaskId, int readyStatusId, int defaultStatusId)
    {
        return allTasks.Select(task => new WeeklistTaskLink
        {
            WeeklistId = weeklistId,
            WeeklistTaskId = task.Id,
            WeeklistTaskStatusId = task.Id == firstTaskId ? readyStatusId : defaultStatusId
        }).ToList();
    }
}