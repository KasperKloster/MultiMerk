using Microsoft.EntityFrameworkCore;
using Application.Repositories.Weeklists;
using Domain.Entities.Weeklists.WeeklistTasks;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Weeklists;
public class WeeklistTaskRepository : IWeeklistTaskRepository
{
    private readonly AppDbContext _dbContext;
    public WeeklistTaskRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<WeeklistTask>> GetAllAsync()
    {
        return await _dbContext.WeeklistTasks.ToListAsync();
    }
}
