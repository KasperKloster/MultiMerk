using System;
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
                WeeklistTaskId = (int)WeeklistTaskName.AssignEAN, // "AssignEAN"
                WeeklistTaskStatusId = (int)WeeklistTaskStatus.Ready, // "Ready"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin  
            },
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskName.InsertOutOfStock, // InsertOutOfStock
                WeeklistTaskStatusId = (int)WeeklistTaskStatus.Ready, // "Ready"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin  
            },     
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskName.GetAIContentList, // GetAIContentList                
                WeeklistTaskStatusId = (int)WeeklistTaskStatus.Ready, // "Ready"
                AssignedUserId = "00000000-0000-0000-0000-000000000004", // Writer
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskName.UploadAIContent, // UploadAIContent
                WeeklistTaskStatusId = (int)WeeklistTaskStatus.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000004", // Writer
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskName.CreateChecklist,
                WeeklistTaskStatusId = (int)WeeklistTaskStatus.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000005", // WarehouseWorker
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskName.InsertWarehouseList,
                WeeklistTaskStatusId = (int)WeeklistTaskStatus.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000006", // WarehouseManager
            },     
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskName.ImportProductList,
                WeeklistTaskStatusId = (int)WeeklistTaskStatus.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin
            },   
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = (int)WeeklistTaskName.CreateTranslations,
                WeeklistTaskStatusId = (int)WeeklistTaskStatus.Awaiting, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin
            }                                
        );
    }
}
