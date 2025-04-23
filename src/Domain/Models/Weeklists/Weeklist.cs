using System.ComponentModel.DataAnnotations;
using Domain.Models.Products;
using Domain.Models.Weeklists.WeeklistTaskLinks;

namespace Domain.Models.Weeklists;

public class Weeklist
{
    [Required]
    public int Id { get; set; }
    
    [Required]    
    public int Number { get; set; }
        
    public string OrderNumber { get; set; } = string.Empty;
        
    public string Supplier { get; set; } = string.Empty;
    
    // Relationsship
    // Has many products
    public ICollection<Product> Products { get; set; } = new List<Product>();

    // Join table for Tasks, and status
    public ICollection<WeeklistTaskLink> WeeklistTaskLinks{ get; set; } = new List<WeeklistTaskLink>();
}
