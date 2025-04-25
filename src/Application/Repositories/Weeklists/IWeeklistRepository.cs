using Domain.Models.Weeklists.Entities;

namespace Application.Repositories.Weeklists;

public interface IWeeklistRepository
{
    Task<List<Weeklist>> GetAllWeeklists();
    Task AddAsync(Weeklist weeklist);    
}