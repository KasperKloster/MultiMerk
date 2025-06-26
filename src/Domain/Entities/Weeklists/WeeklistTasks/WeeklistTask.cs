using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Weeklists.WeeklistTasks;
public class WeeklistTask
{
    [Required]
    public int Id { get; set; }    
    [Required]
    public string Name { get; set; } = string.Empty;    
    public ICollection<WeeklistTaskUserRoleAssignment> UserRoleAssignments { get; set; } = new List<WeeklistTaskUserRoleAssignment>();
}