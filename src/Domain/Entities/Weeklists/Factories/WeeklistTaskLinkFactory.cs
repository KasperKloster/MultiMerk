using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Domain.Entities.Weeklists.WeeklistTasks;
using Domain.Enums;

namespace Domain.Entities.Weeklists.Factories;

public static class WeeklistTaskLinkFactory
{
    public static List<WeeklistTaskLink> CreateLinks(int weeklistId, List<WeeklistTask> tasks, Dictionary<string, string> userRoleToUserId, Dictionary<int, string> taskIdToRole)
    {
        // Define which task IDs should get ready status        
        var readyTaskIds = new HashSet<int> { (int)WeeklistTaskNameEnum.AssignEAN, (int)WeeklistTaskNameEnum.InsertOutOfStock, (int)WeeklistTaskNameEnum.GetAIContentList };
        int readyStatusId = (int)WeeklistTaskStatusEnum.Ready;
        int defaultStatusId = (int)WeeklistTaskStatusEnum.Awaiting;

        return tasks.Select(task =>
        {
            var statusId = readyTaskIds.Contains(task.Id) ? readyStatusId : defaultStatusId;

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