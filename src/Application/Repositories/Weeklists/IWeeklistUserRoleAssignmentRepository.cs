using Domain.Entities.Weeklists.WeeklistTasks;

namespace Application.Repositories.Weeklists;

public interface IWeeklistUserRoleAssignmentRepository 
{
    Task<List<WeeklistTaskUserRoleAssignment>> GetAsync();
}
