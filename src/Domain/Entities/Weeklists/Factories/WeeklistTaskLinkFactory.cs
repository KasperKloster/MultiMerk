using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Domain.Entities.Weeklists.WeeklistTasks;

namespace Domain.Entities.Weeklists.Factories;

public static class WeeklistTaskLinkFactory
{
    public static List<WeeklistTaskLink> CreateLinks(
        int weeklistId,
        List<WeeklistTask> tasks,
        int firstTaskId,
        int readyStatusId,
        int defaultStatusId,
        Dictionary<string, string> userRoleToUserId,
        Dictionary<int, string> taskIdToRole)
    {
        return tasks.Select(task =>
        {
            var statusId = task.Id == firstTaskId ? readyStatusId : defaultStatusId;

            string? assignedUserId = null;
            if (taskIdToRole.TryGetValue(task.Id, out var role))
            {
                userRoleToUserId.TryGetValue(role, out assignedUserId);
            }

            return new WeeklistTaskLink
            {
                WeeklistId = weeklistId,
                WeeklistTaskId = task.Id,
                WeeklistTaskStatusId = statusId,
                AssignedUserId = assignedUserId
            };
        }).ToList();
    }
}