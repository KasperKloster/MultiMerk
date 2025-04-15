using System.ComponentModel.DataAnnotations;
using Domain.Models.Products;

namespace Domain.Models.Weeklists;

public class Weeklist
{
    [Required]
    public int Id { get; set; }
    
    [Required]    
    public int Number { get; set; }
        
    public string OrderNumber { get; set; }
        
    public string Supplier { get; set; }
    
    // Relationsship
    // Has many products
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
