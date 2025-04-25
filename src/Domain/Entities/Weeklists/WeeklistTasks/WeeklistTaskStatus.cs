using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Weeklists.WeeklistTasks;

public class WeeklistTaskStatus
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Status { get; set; } = string.Empty;    
}
