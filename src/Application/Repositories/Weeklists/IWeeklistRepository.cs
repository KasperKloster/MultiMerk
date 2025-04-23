using Domain.Models.Weeklists;

namespace Application.Repositories.Weeklists;

public interface IWeeklistRepository
{
    Task AddAsync(Weeklist weeklist);
}