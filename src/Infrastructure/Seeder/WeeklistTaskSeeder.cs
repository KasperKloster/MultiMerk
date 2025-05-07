using System;
using Domain.Entities.Weeklists.WeeklistTasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Seeder;

public class WeeklistTaskSeeder
{
    public static void Seed(EntityTypeBuilder<WeeklistTask> builder)
    {
        // Seed Products related to Weeklists
        builder.HasData(
            new WeeklistTask { Id = 1, Name = "Assign EAN" },
            new WeeklistTask { Id = 2, Name = "Insert out of stock" },
            new WeeklistTask { Id = 3, Name = "Get AI content list" },
            new WeeklistTask { Id = 4, Name = "Upload AI content" },
            new WeeklistTask { Id = 5, Name = "Create Checklist" },
            new WeeklistTask { Id = 6, Name = "Insert warehouse list" },            
            new WeeklistTask { Id = 7, Name = "Import product list" },
            new WeeklistTask { Id = 8, Name = "Create translations" }
        );
    }
}
