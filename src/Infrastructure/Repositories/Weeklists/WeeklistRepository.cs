using Application.Repositories.Weeklists;
using Domain.Entities.Weeklists.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Weeklists;
public class WeeklistRepository : IWeeklistRepository
{
    private readonly AppDbContext _dbContext;

    public WeeklistRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;        
    }
    public async Task<List<Weeklist>> GetAllWeeklists()
    {
        return await _dbContext.Weeklists
            .Include(w => w.WeeklistTaskLinks)
                .ThenInclude(link => link.WeeklistTask)
            .Include(w => w.WeeklistTaskLinks)
                .ThenInclude(link => link.WeeklistTaskStatus)
            .Include(w => w.WeeklistTaskLinks)
                .ThenInclude(link => link.AssignedUser)                
            .ToListAsync();
    }

    public async Task<Weeklist> GetWeeklist(int weeklistId)
    {
        return await _dbContext.Weeklists.FirstAsync(w => w.Id == weeklistId);        
    }
    
    public async Task AddAsync(Weeklist weeklist)
    {
        await _dbContext.Weeklists.AddAsync(weeklist);
        await _dbContext.SaveChangesAsync();
    }
}
