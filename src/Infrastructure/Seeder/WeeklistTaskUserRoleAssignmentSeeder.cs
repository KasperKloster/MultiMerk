using System;
using Domain.Constants;
using Domain.Entities.Weeklists.WeeklistTasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Seeder;

public class WeeklistTaskUserRoleAssignmentSeeder
{
    public static void Seed(EntityTypeBuilder<WeeklistTaskUserRoleAssignment> builder)
    {
        var userRoleAssignments = new List<WeeklistTaskUserRoleAssignment> {
            new WeeklistTaskUserRoleAssignment{
                Id = 1,
                UserRole = Roles.Admin,
                WeeklistTaskId = 1
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 2,
                UserRole = Roles.Writer,
                WeeklistTaskId = 2
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 3,
                UserRole = Roles.WarehouseWorker,
                WeeklistTaskId = 3
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 4,
                UserRole = Roles.WarehouseWorker,
                WeeklistTaskId = 4
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 5,
                UserRole = Roles.Writer,
                WeeklistTaskId = 5
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 6,
                UserRole = Roles.Admin,
                WeeklistTaskId = 6
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 7,
                UserRole = Roles.Admin,
                WeeklistTaskId = 7
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 8,
                UserRole = Roles.Admin,
                WeeklistTaskId = 8
            },
        };
        builder.HasData(userRoleAssignments);

    }
}
