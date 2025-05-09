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
                WeeklistTaskId = (int)WeeklistTaskNameEnum.AssignEAN, // "AssignEAN"
                WeeklistTaskStatusId = (int)WeeklistTaskStatusEnum.Ready, // "Ready"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin  
            },
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.InsertOutOfStock, // InsertOutOfStock
                WeeklistTaskStatusId = (int)WeeklistTaskStatusEnum.Ready, // "Ready"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin  
            },     
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.GetAIContentList, // GetAIContentList                
                WeeklistTaskStatusId = (int)WeeklistTaskStatusEnum.Ready, // "Ready"
                AssignedUserId = "00000000-0000-0000-0000-000000000004", // Writer
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.UploadAIContent, // UploadAIContent
                WeeklistTaskStatusId = (int)WeeklistTaskStatusEnum.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000004", // Writer
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.CreateChecklist,
                WeeklistTaskStatusId = (int)WeeklistTaskStatusEnum.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000005", // WarehouseWorker
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.InsertWarehouseList,
                WeeklistTaskStatusId = (int)WeeklistTaskStatusEnum.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000006", // WarehouseManager
            },     
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskNameEnum.ImportProductList,
                WeeklistTaskStatusId = (int)WeeklistTaskStatusEnum.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin
            }                            
        );
    }
}
