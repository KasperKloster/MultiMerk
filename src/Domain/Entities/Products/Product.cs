using System.ComponentModel.DataAnnotations;
using Domain.Entities.Weeklists.Entities;

namespace Domain.Entities.Products;

public class Product
{
    public Product(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("SKU cannot be null or empty.", nameof(sku));
        }
        Sku = sku;
    }

    [Required]
    public int Id { get; set; }
    [Required]
    public string Sku { get; set; }
    
    private string? _title;
    public string? Title
    {
        get => _title;
        set
        {
            _title = value;
            AfterLastDash = ExtractAfterLastDash(value);
        }
    }
    public string? AfterLastDash { get; private set; }
    
    // Optionals
    public string? SupplierSku { get; set; }
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
    public string? Location { get; set; }

    // Templates
    public int? TemplateId { get; set; }
    public string? TemplateTitle { get; set; }
    public string? TemplateDescription { get; set; }

    // Relationsship - Has one weeklist    
    public int? WeeklistId { get; set; } // Foreign key
    public Weeklist? Weeklist { get; set; }

    // Entity helpers
    private string? ExtractAfterLastDash(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }
        var lastDashIndex = input.LastIndexOf('-');
        if (lastDashIndex == -1 || lastDashIndex == input.Length - 1)
        {
            return string.Empty;
        }
        return input.Substring(lastDashIndex + 1).Trim();
    }
}
