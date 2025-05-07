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
                WeeklistTaskId = (int)WeeklistTaskNameEnum.AssignEAN
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 2,
                UserRole = Roles.Admin,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.InsertOutOfStock
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 3,
                UserRole = Roles.Writer,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.GetAIContentList
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 4,
                UserRole = Roles.Writer,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.UploadAIContent
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 5,
                UserRole = Roles.WarehouseWorker,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.CreateChecklist
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 6,
                UserRole = Roles.WarehouseManager,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.InsertWarehouseList
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 7,
                UserRole = Roles.Admin,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.ImportProductList
            },
            new WeeklistTaskUserRoleAssignment
            {
                Id = 8,
                UserRole = Roles.Admin,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.CreateTranslations
            },                   
        };
        builder.HasData(userRoleAssignments);

    }
}
