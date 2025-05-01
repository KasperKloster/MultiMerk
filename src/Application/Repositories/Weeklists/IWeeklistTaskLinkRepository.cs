using Domain.Entities.Weeklists.WeeklistTaskLinks;

namespace Application.Repositories.Weeklists;

public interface IWeeklistTaskLinkRepository
{
    Task AddWeeklistTaskLinksAsync(List<WeeklistTaskLink> taskLinks);
    Task UpdateTaskStatus();
}
