using Domain.Models.Weeklists.WeeklistTasks;

namespace Application.Repositories.Weeklists;

public interface IWeeklistTaskRepository
{
    Task<List<WeeklistTask>> GetAllAsync();
}
