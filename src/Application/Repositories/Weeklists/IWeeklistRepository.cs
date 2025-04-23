using Domain.Models.Weeklists;
using Domain.Models.Weeklists.WeeklistTaskLinks;

namespace Application.Repositories.Weeklists;

public interface IWeeklistRepository
{
    Task AddAsync(Weeklist weeklist);    
}