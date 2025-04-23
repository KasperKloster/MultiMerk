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
    
    foreach (var link in taskLinks)
    {
        Console.WriteLine($"Saving Link: WL={link.WeeklistId}, Task={link.WeeklistTaskId}, Status={link.WeeklistTaskStatusId}");
        // Optional: check for null navs
        if (link.Weeklist == null && link.WeeklistId == 0)
            throw new Exception("Weeklist is null or Id is 0");
    }

        await _dbContext.WeeklistTaskLinks.AddRangeAsync(taskLinks);
        await _dbContext.SaveChangesAsync();
    }
}