using System.ComponentModel.DataAnnotations;

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
}
