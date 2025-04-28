using System;
using Domain.Constants;
using Domain.Entities.Weeklists.WeeklistTasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Seeder;

public class WeeklistTaskAssignmentSeeder
{
    public static void Seed(EntityTypeBuilder<WeeklistTaskAssignment> builder)
    {
        var assignments = new List<WeeklistTaskAssignment>
        {
            new WeeklistTaskAssignment
            {
                Id = 1,
                UserRole = Roles.Admin,
                WeeklistTaskId = 1
            },
            new WeeklistTaskAssignment
            {
                Id = 2,
                UserRole = Roles.Writer,
                WeeklistTaskId = 2
            },
            new WeeklistTaskAssignment
            {
                Id = 3,
                UserRole = Roles.WarehouseWorker,
                WeeklistTaskId = 3
            },
            new WeeklistTaskAssignment
            {
                Id = 4,
                UserRole = Roles.WarehouseWorker,
                WeeklistTaskId = 4
            },
            new WeeklistTaskAssignment
            {
                Id = 5,
                UserRole = Roles.Writer,
                WeeklistTaskId = 5
            },     
            new WeeklistTaskAssignment
            {
                Id = 6,
                UserRole = Roles.Admin,
                WeeklistTaskId = 6
            },
            new WeeklistTaskAssignment
            {
                Id = 7,
                UserRole = Roles.Admin,
                WeeklistTaskId = 7
            },
            new WeeklistTaskAssignment
            {
                Id = 8,
                UserRole = Roles.Admin,
                WeeklistTaskId = 8
            },                        
        };

        builder.HasData(assignments);
    }
}
