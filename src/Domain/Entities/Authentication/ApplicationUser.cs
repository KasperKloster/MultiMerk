using System;
using Domain.Entities.Weeklists.WeeklistTasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Authentication;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;

    public ICollection<WeeklistTaskAssignment> WeeklistTaskAssignments { get; set; } = new List<WeeklistTaskAssignment>();
}
