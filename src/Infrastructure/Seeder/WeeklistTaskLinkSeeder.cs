using System;
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
                WeeklistTaskId = 1, // "Assign EAN"
                WeeklistTaskStatusId = 2, // "Ready"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin  
            },
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = 2,
                WeeklistTaskStatusId = 1, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000004", // Writer
            },     
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = 3,
                WeeklistTaskStatusId = 1, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000005", // WarehouseWorker
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = 4,
                WeeklistTaskStatusId = 1, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000005", // WarehouseWorker
            },             
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = 5,
                WeeklistTaskStatusId = 1, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000004", // Writer
            },   
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = 6,
                WeeklistTaskStatusId = 1, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = 7,
                WeeklistTaskStatusId = 1, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin
            },     
            new WeeklistTaskLink
            {
                WeeklistId = 1,
                WeeklistTaskId = 8,
                WeeklistTaskStatusId = 1, // "Awaiting"
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin
            },       
            // Link Weeklist 2
            new WeeklistTaskLink
            {
                WeeklistId = 2,
                WeeklistTaskId = 1, // "Assign EAN"
                WeeklistTaskStatusId = 4,
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin
            },
            new WeeklistTaskLink
            {
                WeeklistId = 2,
                WeeklistTaskId = 2,
                WeeklistTaskStatusId = 4,
                AssignedUserId = "00000000-0000-0000-0000-000000000004", // Writer
            },     
            new WeeklistTaskLink
            {
                WeeklistId = 2,
                WeeklistTaskId = 3,
                WeeklistTaskStatusId = 4,
                AssignedUserId = "00000000-0000-0000-0000-000000000005", // WarehouseWoker
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 2,
                WeeklistTaskId = 4,
                WeeklistTaskStatusId = 4,
                AssignedUserId = "00000000-0000-0000-0000-000000000005", // WarehouseWoker
            },             
            new WeeklistTaskLink
            {
                WeeklistId = 2,
                WeeklistTaskId = 5,
                WeeklistTaskStatusId = 4,
                AssignedUserId = "00000000-0000-0000-0000-000000000004", // Writer
            },   
            new WeeklistTaskLink
            {
                WeeklistId = 2,
                WeeklistTaskId = 6,
                WeeklistTaskStatusId = 4,
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin
            }, 
            new WeeklistTaskLink
            {
                WeeklistId = 2,
                WeeklistTaskId = 7,
                WeeklistTaskStatusId = 3, 
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin
            },     
            new WeeklistTaskLink
            {
                WeeklistId = 2,
                WeeklistTaskId = 8,
                WeeklistTaskStatusId = 1,
                AssignedUserId = "00000000-0000-0000-0000-000000000001", // Admin
            }                            
        );
    }
}
