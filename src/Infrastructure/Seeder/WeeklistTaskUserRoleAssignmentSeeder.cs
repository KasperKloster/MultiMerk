using System;
using Domain.Constants;
using Domain.Enums;
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
                WeeklistTaskId = (int)TaskNameEnum.AssignEAN
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 2,
                UserRole = Roles.Admin,
                WeeklistTaskId = (int)TaskNameEnum.InsertOutOfStock
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 3,
                UserRole = Roles.Writer,
                WeeklistTaskId = (int)TaskNameEnum.GetAIContentList
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 4,
                UserRole = Roles.Writer,
                WeeklistTaskId = (int)TaskNameEnum.UploadAIContent
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 5,
                UserRole = Roles.WarehouseWorker,
                WeeklistTaskId = (int)TaskNameEnum.CreateChecklist
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 6,
                UserRole = Roles.WarehouseManager,
                WeeklistTaskId = (int)TaskNameEnum.InsertWarehouseList
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 7,
                UserRole = Roles.Admin,
                WeeklistTaskId = (int)TaskNameEnum.ImportProductList
            }                 
        };
        builder.HasData(userRoleAssignments);

    }
}
