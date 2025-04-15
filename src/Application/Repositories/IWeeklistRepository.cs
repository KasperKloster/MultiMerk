using Domain.Models.Weeklists;

namespace Application.Repositories;

public interface IWeeklistRepository
{
    Task AddAsync(Weeklist weeklist);
}