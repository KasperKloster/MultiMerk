using System;
using Domain.Entities.Products;
using Domain.Entities.Weeklists.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Seeder;

public class WeeklistSeeder
{
    public static void Seed(EntityTypeBuilder<Weeklist> builder)
    {
        // Seed weeklist
        builder.HasData(
            new Weeklist
            {
                Id = 1,
                Number = 101,
                OrderNumber = "E123",
                Supplier = "TVC",
                ShippingNumber = "Shipment101"
            }
        );

    }
}

