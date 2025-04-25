using Domain.Entities.Weeklists.WeeklistTasks;

namespace Application.Repositories.Weeklists;

public interface IWeeklistTaskRepository
{
    Task<List<WeeklistTask>> GetAllAsync();
}
