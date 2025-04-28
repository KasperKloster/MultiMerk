using Domain.Entities.Authentication;

namespace Domain.Entities.Weeklists.WeeklistTasks;

public class WeeklistTaskAssignment
{
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }

    public int WeeklistTaskId { get; set; }
    public WeeklistTask? WeeklistTask { get; set; }
}
