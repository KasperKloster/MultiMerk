using System;
using Domain.Entities.Weeklists.WeeklistTasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Seeder;

public class WeeklistTaskStatusSeeder
{
    public static void Seed(EntityTypeBuilder<WeeklistTaskStatus> builder)
    {
        // Seed Products related to Weeklists
        builder.HasData(
            new WeeklistTaskStatus { Id = 1, Status = "Awaiting" },
            new WeeklistTaskStatus { Id = 2, Status = "Ready" },
            new WeeklistTaskStatus { Id = 3, Status = "In Progress" },
            new WeeklistTaskStatus { Id = 4, Status = "Done" }
        );    
    }


}
