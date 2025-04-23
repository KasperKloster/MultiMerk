using Domain.Models.Weeklists.WeeklistTaskLinks;

namespace Application.Repositories.Weeklists;

public interface IWeeklistTaskLinkRepository
{
    Task AddWeeklistTaskLinksAsync(List<WeeklistTaskLink> taskLinks);
}
