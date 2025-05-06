using Domain.Entities.Weeklists.Entities;

namespace Application.Repositories.Weeklists;

public interface IWeeklistRepository
{
    Task<List<Weeklist>> GetAllWeeklists();
    Task<Weeklist> GetWeeklist(int weeklistId);
    Task AddAsync(Weeklist weeklist);    
}