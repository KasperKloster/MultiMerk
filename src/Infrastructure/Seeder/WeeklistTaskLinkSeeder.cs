using Domain.Enums;
using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Seeder;

public class WeeklistTaskLinkSeeder
{
    public static void Seed(EntityTypeBuilder<WeeklistTaskLink> builder)
    {
        builder.HasData(
            // Link Weeklist 1
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)TaskNameEnum.AssignEAN, // "AssignEAN"
                WeeklistTaskStatusId = (int)TaskStatusEnum.Ready, // "Ready"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin  
            },
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)TaskNameEnum.InsertOutOfStock, // InsertOutOfStock
                WeeklistTaskStatusId = (int)TaskStatusEnum.Ready, // "Ready"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin  
            },     
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)TaskNameEnum.GetAIContentList, // GetAIContentList                
                WeeklistTaskStatusId = (int)TaskStatusEnum.Ready, // "Ready"
                AssignedUserId = "00000000-0000-0000-0000-000000000004", // Writer
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)TaskNameEnum.UploadAIContent, // UploadAIContent
                WeeklistTaskStatusId = (int)TaskStatusEnum.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000004", // Writer
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)TaskNameEnum.CreateChecklist,
                WeeklistTaskStatusId = (int)TaskStatusEnum.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000005", // WarehouseWorker
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)TaskNameEnum.InsertWarehouseList,
                WeeklistTaskStatusId = (int)TaskStatusEnum.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000006", // WarehouseManager
            },     
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)TaskNameEnum.ImportProductList,
                WeeklistTaskStatusId = (int)TaskStatusEnum.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin
            }                            
        );
    }
}
