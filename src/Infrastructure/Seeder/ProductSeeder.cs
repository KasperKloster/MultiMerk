using System;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Seeder;

public class ProductSeeder
{
    public static void Seed(EntityTypeBuilder<Product> builder)
    {
        // Seed Products related to Weeklists
        builder.HasData(
            // Links to Weeklist with Id 1
            new Product(sku : "LC01-1001-1")
            {
                Id = 1,                
                WeeklistId = 1,
                Title = "Product One",
                Qty = 0
            },
            new Product(sku : "LC01-1001-2")
            {
                Id = 2,                
                WeeklistId = 1,
                Title = "Product Two",
                Qty = 3
            }
        );
    }
}
