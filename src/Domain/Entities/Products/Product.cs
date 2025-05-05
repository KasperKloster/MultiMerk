using System.ComponentModel.DataAnnotations;
using Domain.Entities.Weeklists.Entities;

namespace Domain.Entities.Products;

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
    // Optionals
    public string? SupplierSku { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? EAN { get; set; }
    public string? CategoryId { get; set; }
    public string? Series { get; set; }
    public string? Color { get; set; }
    public string? Material { get; set; }
    public int? Price { get; set; }
    public float? Cost { get; set; }
    public int? Qty { get; set; }
    public float? Weight { get; set; }
    public string? MainImage { get; set; }
    public int? TemplateId { get; set; }
    public string? Location { get; set; }

    // Relationsship
    // Has one weeklist
    public int? WeeklistId { get; set; } // Foreign key
    public Weeklist? Weeklist { get; set; }
}
