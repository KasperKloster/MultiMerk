namespace Domain.Entities.Weeklists.WeeklistTasks;

public class WeeklistTaskUserRoleAssignment
{
    public int Id { get; set; }
    public string UserRole { get; set; } = string.Empty;
    public int WeeklistTaskId { get; set; }
    public WeeklistTask? WeeklistTask { get; set; } = null!;    
}
