using System.ComponentModel.DataAnnotations;
using Domain.Models.Weeklists;

namespace Domain.Models.Products;

public class Product
{
    public Product(string sku)
    {
        Sku = sku;
    }
    
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Sku { get; set; }

    // Relationsship
    // Has one weeklist
    public int? WeeklistId { get; set; } // Foreign key
    public Weeklist? Weeklist { get; set; }
}