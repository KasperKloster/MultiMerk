using Domain.Entities.Authentication;

namespace Domain.Entities.Weeklists.WeeklistTasks;

public class WeeklistTaskAssignment
{
    public int Id { get; set; }
    public string UserRole = string.Empty;
    public int WeeklistTaskId { get; set; }
    public WeeklistTask? WeeklistTask { get; set; }
}
