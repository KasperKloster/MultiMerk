using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Weeklists.WeeklistTasks;

public class WeeklistTask
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
}
