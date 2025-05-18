using System;
using Application.Repositories.Weeklists;
using Domain.Entities.Weeklists.WeeklistTasks;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Weeklists;
public class WeeklistUserRoleAssignmentRepository : IWeeklistUserRoleAssignmentRepository
{
    private readonly AppDbContext _dbContext;
    public WeeklistUserRoleAssignmentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<WeeklistTaskUserRoleAssignment>> GetAsync()
    {
        return await _dbContext.WeeklistTaskUserRoleAssignments
            .Include(w => w.WeeklistTask)
            .ToListAsync();        
    }
}
