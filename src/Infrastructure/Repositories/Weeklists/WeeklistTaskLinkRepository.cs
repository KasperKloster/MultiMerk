using System.Diagnostics;
using Application.Repositories.Weeklists;
using Domain.Models.Weeklists.WeeklistTaskLinks;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Weeklists;

public class WeeklistTaskLinkRepository : IWeeklistTaskLinkRepository
{
    private readonly AppDbContext _dbContext;

    public WeeklistTaskLinkRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddWeeklistTaskLinksAsync(List<WeeklistTaskLink> taskLinks)
    {
        await _dbContext.WeeklistTaskLinks.AddRangeAsync(taskLinks);
        await _dbContext.SaveChangesAsync();
    }
}