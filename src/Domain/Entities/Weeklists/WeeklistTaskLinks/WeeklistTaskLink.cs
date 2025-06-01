using Domain.Entities.Authentication;
using Domain.Entities.Weeklists.Entities;
using Domain.Entities.Weeklists.WeeklistTasks;

namespace Domain.Entities.Weeklists.WeeklistTaskLinks;
public class WeeklistTaskLink
{
    public int WeeklistId { get; set; }
 
    public Weeklist? Weeklist { get; set; }

    public int WeeklistTaskId { get; set; }
    public WeeklistTask? WeeklistTask { get; set; }

    public int WeeklistTaskStatusId { get; set; }
    public WeeklistTaskStatus? WeeklistTaskStatus { get; set; }

    public string? AssignedUserId { get; set; }
    public ApplicationUser? AssignedUser { get; set; }
}
