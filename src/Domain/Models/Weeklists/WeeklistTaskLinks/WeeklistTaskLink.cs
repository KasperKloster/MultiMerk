using Domain.Models.Weeklists.Entities;
using Domain.Models.Weeklists.WeeklistTasks;

namespace Domain.Models.Weeklists.WeeklistTaskLinks;

public class WeeklistTaskLink
{
    public int WeeklistId { get; set; }
    
    public Weeklist? Weeklist { get; set; }

    public int WeeklistTaskId { get; set; }
    public WeeklistTask? WeeklistTask { get; set; }

    public int WeeklistTaskStatusId { get; set; }
    public WeeklistTaskStatus? WeeklistTaskStatus { get; set; }
}
