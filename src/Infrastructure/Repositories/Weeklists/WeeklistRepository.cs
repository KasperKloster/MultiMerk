using System.Diagnostics;
using Application.Repositories.Weeklists;
using Domain.Models.Weeklists;
using Domain.Models.Weeklists.WeeklistTaskLinks;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Weeklists;

public class WeeklistRepository : IWeeklistRepository
{
    private readonly AppDbContext _dbContext;

    public WeeklistRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;        
    }

    public async Task AddAsync(Weeklist weeklist)
    {
        await _dbContext.Weeklists.AddAsync(weeklist);
        await _dbContext.SaveChangesAsync();
    }
}
